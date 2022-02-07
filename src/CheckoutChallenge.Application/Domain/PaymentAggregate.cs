using System.Threading.Tasks;
using CheckoutChallenge.Application.Acquirers;
using CheckoutChallenge.Application.Domain.Events;
using CheckoutChallenge.Application.PaymentProcessing;

namespace CheckoutChallenge.Application.Domain
{
    public class PaymentAggregate : AggregateRoot<MerchantPaymentId>
    {
        private readonly string _merchantRef;
        private readonly int _amount;
        private readonly string _currency;
        private readonly string _expiry;
        private readonly string _cvv;
        private readonly string _pan;
        private readonly string _cardHolderName;

        public PaymentAggregate(MerchantPaymentId id, string merchantRef, int amount, string currency, string expiry, string cvv, string pan, string cardHolderName) : base(id)
        {
            _merchantRef = merchantRef;
            _amount = amount;
            _currency = currency;
            _expiry = expiry;
            _cvv = cvv;
            _pan = pan;
            _cardHolderName = cardHolderName;

            ApplyChange(new PaymentCreated(id, merchantRef, amount, currency, expiry, cvv, pan, cardHolderName));
        }

        public async Task<PaymentResponse> Authorise(IAcquirer acquirer)
        {
            var request = new AuthorisationRequest(_amount, _currency, _expiry, _cvv, _pan, _cardHolderName);

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
