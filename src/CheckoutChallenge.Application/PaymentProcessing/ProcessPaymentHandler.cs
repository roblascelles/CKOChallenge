using System;
using System.Threading.Tasks;
using CheckoutChallenge.Application.Acquirers;
using CheckoutChallenge.Application.DataStore;
using CheckoutChallenge.Application.Domain;

namespace CheckoutChallenge.Application.PaymentProcessing
{
    public class ProcessPaymentHandler
    {
        private readonly IAcquirer _acquirer;
        private readonly IRepository<PaymentAggregate, MerchantPaymentId> _repository;

        public ProcessPaymentHandler(IAcquirer acquirer, IRepository<PaymentAggregate, MerchantPaymentId> repository)
        {
            _acquirer = acquirer;
            _repository = repository;
        }

        public async Task<PaymentResponse> HandleAsync(ProcessPaymentCommand command)
        {
            var id = MerchantPaymentId.CreateNew(command.MerchantId);

            var payment = new PaymentAggregate(id, command.MerchantRef, command.Amount, command.Currency, command.Expiry, command.Cvv, command.Pan, command.CardHolderName);

            var response = await payment.Authorise(_acquirer);

            await _repository.SaveAsync(payment);

            return response;
        }

    }
}
