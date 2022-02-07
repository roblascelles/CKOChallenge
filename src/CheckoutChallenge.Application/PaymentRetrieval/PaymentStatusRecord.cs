using CheckoutChallenge.Application.Domain;
using CheckoutChallenge.Application.PaymentProcessing;

namespace CheckoutChallenge.Application.PaymentRetrieval
{
    public class PaymentStatusRecord
    {
        public MerchantPaymentId Id { get; }
        public string? AuthCode { get; }
        public PaymentStatus PaymentStatus { get; }

        public PaymentStatusRecord(MerchantPaymentId id, PaymentStatus paymentStatus, string? authCode = null)
        {
            Id = id;
            PaymentStatus = paymentStatus;
            AuthCode = authCode;
        }

    }
}