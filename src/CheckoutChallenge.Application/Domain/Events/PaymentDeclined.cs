using CheckoutChallenge.Application.Acquirers;
using CheckoutChallenge.Application.PaymentProcessing;

namespace CheckoutChallenge.Application.Domain.Events
{
    public class PaymentDeclined : Event<MerchantPaymentId>
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