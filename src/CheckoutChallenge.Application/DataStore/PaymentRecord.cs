using CheckoutChallenge.Application.PaymentProcessing;

namespace CheckoutChallenge.Application.DataStore
{
    public class PaymentRecord
    {
        public PaymentRecord(string id, PaymentStatus status, string authCode)
        {
            Id = id;
            Status = status;
            AuthCode = authCode;
        }
        public string Id { get; }

        public PaymentStatus Status { get; }
        public string AuthCode { get; }
    }
}