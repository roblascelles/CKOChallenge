using CheckoutChallenge.Application.Acquirers;

namespace CheckoutChallenge.Application.Domain
{
    public class PaymentDeclined : MerchantPaymentEvent
    {
        public PaymentDeclined(MerchantPaymentId id, AuthorisationStatus status) : base(id)
        {
            Status = status;
        }
        public AuthorisationStatus Status { get; }

    }
}