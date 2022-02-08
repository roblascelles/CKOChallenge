﻿namespace CheckoutChallenge.Application.Domain.Events
{
    public class PaymentCreated : Event<MerchantPaymentId>
    {
        public PaymentCreated(MerchantPaymentId id, string merchantRef, int amount, string currency, Card card) : base(id)
        {
            MerchantRef = merchantRef;
            Amount = amount;
            Currency = currency;
            Card = card;
        }
        public string MerchantRef { get; }
        public int Amount { get; }
        public string Currency { get; }
        public Card Card{ get; }
    }
}
