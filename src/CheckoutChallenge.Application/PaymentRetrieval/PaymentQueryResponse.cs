using CheckoutChallenge.Application.PaymentProcessing;

namespace CheckoutChallenge.Application.PaymentRetrieval
{
    public class PaymentQueryResponse
    {
        public PaymentQueryResponse(string paymentId, PaymentStatus status, string merchantRef, int amount, string currency, string cardBin, string cardLast4Digits,  string cardExpiry, string cardHolder, string? authCode)
        {
            PaymentId = paymentId;
            Status = status;
            MerchantRef = merchantRef;

            Amount = amount;
            Currency = currency;

            CardBin = cardBin;
            CardLast4Digits = cardLast4Digits;

            CardExpiry = cardExpiry;
            CardHolder = cardHolder;

            AuthCode = authCode;
        }
        public string PaymentId { get; }
        public PaymentStatus Status { get; }
        public string? AuthCode { get; }
        public int Amount { get; set; }
        public string Currency { get; set; }
        public string MerchantRef { get; set; }
        public string CardBin { get; set; }
        public string CardLast4Digits { get; set; }
        public string CardExpiry { get; set; }
        public string CardHolder { get; set; }
    }
}