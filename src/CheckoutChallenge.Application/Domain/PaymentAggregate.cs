using System.Threading.Tasks;
using CheckoutChallenge.Application.Acquirers;
using CheckoutChallenge.Application.Domain.Events;
using CheckoutChallenge.Application.PaymentProcessing;

namespace CheckoutChallenge.Application.Domain
{
    public class PaymentAggregate : AggregateRoot<MerchantPaymentId>
    {
        private string _merchantRef;
        private int _amount;
        private string _currency;
        private Card _card;

        public PaymentAggregate(MerchantPaymentId id, string merchantRef, int amount, string currency, Card card) : base(id)
        {
            ApplyChange(new PaymentCreated(id, merchantRef, amount, currency, card));
        }

        public async Task<PaymentResponse> Authorise(IAcquirer acquirer)
        {
            var request = new AuthorisationRequest(_amount, _currency, _card);

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

            return new PaymentResponse(Id.PaymentId, approved, _merchantRef, status, response.Amount, response.Currency, response.AuthCode);
        }

        internal void Apply(PaymentCreated @event)
        {
            _merchantRef = @event.MerchantRef;
            _amount = @event.Amount;
            _currency = @event.Currency;
            _card = @event.Card;
        }

        internal void Apply(PaymentAuthorised @event)
        {
        }

        internal void Apply(PaymentDeclined @event)
        {
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
