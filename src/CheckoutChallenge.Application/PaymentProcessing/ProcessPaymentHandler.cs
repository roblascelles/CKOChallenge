using System;
using System.Threading.Tasks;
using CheckoutChallenge.Application.Acquirers;
using CheckoutChallenge.Application.DataStore;

namespace CheckoutChallenge.Application.PaymentProcessing
{
    public class ProcessPaymentHandler
    {
        private readonly IAcquirer _acquirer;
        private readonly IPaymentRepository _repository;

        public ProcessPaymentHandler(IAcquirer acquirer, IPaymentRepository repository)
        {
            _acquirer = acquirer;
            _repository = repository;
        }

        public async Task<PaymentResponse> HandleAsync(ProcessPaymentCommand command)
        {
            var id = Guid.NewGuid().ToString("N");

            var response = await _acquirer.AuthoriseAsync(CreateAuthorisationRequest(command));

            var status = MapPaymentStatus(response.Status);
            var approved = status == PaymentStatus.Authorized;

            await _repository.SaveAsync(command.MerchantId, new PaymentRecord(id, status, response.AuthCode));

            return new PaymentResponse(id, approved, command.MerchantRef, status, response.Amount, response.Currency, response.AuthCode);
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
