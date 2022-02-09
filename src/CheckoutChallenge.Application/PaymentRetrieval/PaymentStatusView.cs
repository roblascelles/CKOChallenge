using System;
using System.Threading.Tasks;
using CheckoutChallenge.Application.Bus;
using CheckoutChallenge.Application.Domain.Events;
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
            await _repository.SaveAsync(@event.AggregateId, new PaymentStatusRecord(@event.AggregateId, PaymentStatus.Processing, @event.MerchantRef, @event.Amount, @event.Currency, @event.Card.Bin, @event.Card.Last4Digits, @event.Card.Expiry, @event.Card.CardHolderName));
        }

        public async Task Handle(PaymentAuthorised @event)
        {
            await _repository.UpdateAuthStatusAsync(@event.AggregateId, PaymentStatus.Authorized, @event.AuthCode);
        }

        public async Task Handle(PaymentDeclined @event)
        {
            await _repository.UpdateAuthStatusAsync(@event.AggregateId, @event.PaymentStatus);
        }

        public void Subscribe(IEventSubscriber bus)
        {
            bus.Subscribe<PaymentCreated>(Handle);
            bus.Subscribe<PaymentAuthorised>(Handle);
            bus.Subscribe<PaymentDeclined>(Handle);
        }

    }
}
