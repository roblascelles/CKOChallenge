namespace CheckoutChallenge.Application.PaymentProcessing
{
    public class PaymentResponse
    {
        public PaymentResponse(bool approved, string merchantRef, PaymentStatus status, int amount, string currency, string authCode)
        {
            Approved = approved;
            MerchantRef = merchantRef;
            Status = status;
            Amount = amount;
            Currency = currency;
            AuthCode = authCode;
        }

        public bool Approved { get; }
        public string MerchantRef { get; }
        public PaymentStatus Status { get; }
        public int Amount { get; }
        public string Currency { get; }
        public string AuthCode { get; }
    }
}
