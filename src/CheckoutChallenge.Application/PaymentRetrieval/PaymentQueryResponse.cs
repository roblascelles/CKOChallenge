using CheckoutChallenge.Application.PaymentProcessing;

namespace CheckoutChallenge.Application.PaymentRetrieval
{
    public class PaymentQueryResponse
    {
        public PaymentQueryResponse(string id, PaymentStatus status, string? authCode)
        {
            Id = id;
            Status = status;
            AuthCode = authCode;
        }
        public string Id { get; }
        public PaymentStatus Status { get; }
        public string? AuthCode { get; }

    }
}