using System;
using System.Threading.Tasks;
using CheckoutChallenge.Application.Acquirers;
using CheckoutChallenge.Application.Bus;
using CheckoutChallenge.Application.DataStore;
using CheckoutChallenge.Application.Domain;

namespace CheckoutChallenge.Application.PaymentProcessing
{
    public class ProcessPaymentHandler
    {
        private readonly IAcquirer _acquirer;
        private readonly IPaymentRepository _repository;
        private readonly IPaymentEventPublisher _eventPublisher;

        public ProcessPaymentHandler(IAcquirer acquirer, IPaymentRepository repository, IPaymentEventPublisher eventPublisher)
        {
            _acquirer = acquirer;
            _repository = repository;
            _eventPublisher = eventPublisher;
        }

        public async Task<PaymentResponse> HandleAsync(ProcessPaymentCommand command)
        {
            var id = MerchantPaymentId.CreateNew(command.MerchantId);

            await _eventPublisher.PublishAsync(new PaymentCreated(id, command.MerchantRef, command.Amount,
                command.Currency, command.Expiry, command.Cvv, command.Pan, command.CardHolderName));

            var response = await _acquirer.AuthoriseAsync(CreateAuthorisationRequest(command));

            var status = MapPaymentStatus(response.Status);
            var approved = status == PaymentStatus.Authorized;

            if (approved)
            {
                await _eventPublisher.PublishAsync(new PaymentAuthorised(id, response.Amount, response.Currency, response.AuthCode));
            }
            else
            {
                await _eventPublisher.PublishAsync(new PaymentDeclined(id, status, response.Status));
            }

            await _repository.SaveAsync(command.MerchantId, new PaymentRecord(id.PaymentId, status, response.AuthCode));

            return new PaymentResponse(id.PaymentId, approved, command.MerchantRef, status, response.Amount, response.Currency, response.AuthCode);
        }

        private static AuthorisationRequest CreateAuthorisationRequest(ProcessPaymentCommand command)
        {
            return new AuthorisationRequest(command.Amount, command.Currency, command.Expiry, command.Cvv, command.Pan, command.CardHolderName);
        }

        private static PaymentStatus MapPaymentStatus(AuthorisationStatus responseStatus)
        {
            switch (responseStatus)
            {
                case AuthorisationStatus.Authorised: return PaymentStatus.Authorized;
                case AuthorisationStatus.Declined:
                case AuthorisationStatus.InsufficientFunds:
                case AuthorisationStatus.CardStolen: return PaymentStatus.Declined;
                case AuthorisationStatus.Error: return PaymentStatus.Error;
                default: return PaymentStatus.Unknown;
            }
        }
    }
}
