﻿using System.Threading.Tasks;
using CheckoutChallenge.Application.Acquirers;
using CheckoutChallenge.Application.Domain.Events;
using CheckoutChallenge.Application.PaymentProcessing;

namespace CheckoutChallenge.Application.Domain
{
    public class PaymentAggregate : AggregateRoot<MerchantPaymentId>
    {
        public string MerchantRef { get; private set; }
        public int Amount { get; private set; }
        public string Currency { get; private set; }
        public CardSummary Card { get; private set; }

        public int? AuthorisedAmount { get; private set; }
        public string? AuthCode { get; private set; }

        public AuthorisationStatus AuthorisationStatus { get; private set; }


        //need for restoring:
        public PaymentAggregate() {}

        public PaymentAggregate(MerchantPaymentId id, string merchantRef, int amount, string currency, CardSummary card) 
        {
            ApplyChange(new PaymentCreated(id, merchantRef, amount, currency, card));
        }

        public async Task<PaymentResponse> Authorise(IAcquirer acquirer, Card card)
        {
            var request = new AuthorisationRequest(Amount, Currency, card);

            var response = await acquirer.AuthoriseAsync(request);

            var status = MapPaymentStatus(response.Status);

            var approved = (status == PaymentStatus.Authorized);

            if (approved)
            {
                ApplyChange(new PaymentAuthorised(Id, response.Amount, response.Currency, response.AuthCode));
            }
            else
            {
                ApplyChange(new PaymentDeclined(Id, status, response.Status));
            }

            return new PaymentResponse(Id.PaymentId, approved, MerchantRef, status, response.Amount, response.Currency, response.AuthCode);
        }

        internal void Apply(PaymentCreated @event)
        {
            Id = @event.AggregateId;
            MerchantRef = @event.MerchantRef;
            Amount = @event.Amount;
            Currency = @event.Currency;
            Card = @event.Card;
        }

        internal void Apply(PaymentAuthorised @event)
        {
            AuthorisedAmount = @event.Amount;
            AuthorisationStatus = AuthorisationStatus.Authorised;
            AuthCode = @event.AuthCode;
        }

        internal void Apply(PaymentDeclined @event)
        {
            AuthorisationStatus = @event.AuthorisationStatus;
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
