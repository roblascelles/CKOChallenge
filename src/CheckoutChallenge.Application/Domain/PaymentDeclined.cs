using CheckoutChallenge.Application.Acquirers;
using CheckoutChallenge.Application.PaymentProcessing;

namespace CheckoutChallenge.Application.Domain
{
    public class PaymentDeclined : MerchantPaymentEvent
    {
        public PaymentDeclined(MerchantPaymentId id, PaymentStatus paymentStatus, AuthorisationStatus authorisationStatus) : base(id)
        {
            PaymentStatus = paymentStatus;
            AuthorisationStatus = authorisationStatus;
        }

        public PaymentStatus PaymentStatus { get; }
        public AuthorisationStatus AuthorisationStatus { get; }

    }
}