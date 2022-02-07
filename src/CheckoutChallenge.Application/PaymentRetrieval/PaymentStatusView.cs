using System;
using System.Threading.Tasks;
using CheckoutChallenge.Application.Bus;
using CheckoutChallenge.Application.Domain;
using CheckoutChallenge.Application.PaymentProcessing;

namespace CheckoutChallenge.Application.PaymentRetrieval
{
    public class PaymentStatusView
    {
        private readonly IPaymentStatusRecordRepository _repository;

        public PaymentStatusView(IPaymentStatusRecordRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(PaymentCreated @event)
        {
            await _repository.SaveAsync(@event.Id, new PaymentStatusRecord(@event.Id, PaymentStatus.Processing));
        }

        public async Task Handle(PaymentAuthorised @event)
        {
            await _repository.SaveAsync(@event.Id, new PaymentStatusRecord(@event.Id, PaymentStatus.Authorized, @event.AuthCode));
        }

        public async Task Handle(PaymentDeclined @event)
        {
            await _repository.SaveAsync(@event.Id, new PaymentStatusRecord(@event.Id, @event.PaymentStatus));
        }

        public void Subscribe(IPaymentEventSubscriber bus)
        {
            bus.Subscribe<PaymentCreated>(Handle);
            bus.Subscribe<PaymentAuthorised>(Handle);
            bus.Subscribe<PaymentDeclined>(Handle);
        }

    }
}
