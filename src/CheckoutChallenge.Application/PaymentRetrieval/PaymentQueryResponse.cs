using CheckoutChallenge.Application.PaymentProcessing;

namespace CheckoutChallenge.Application.PaymentRetrieval
{
    public class PaymentQueryResponse
    {
        public PaymentQueryResponse(string paymentId, PaymentStatus status, string? authCode)
        {
            PaymentId = paymentId;
            Status = status;
            AuthCode = authCode;
        }
        public string PaymentId { get; }
        public PaymentStatus Status { get; }
        public string? AuthCode { get; }

    }
}