using CheckoutChallenge.Application.Domain;
using CheckoutChallenge.Application.PaymentProcessing;

namespace CheckoutChallenge.Application.PaymentRetrieval
{
    public class PaymentStatusRecord
    {
        public MerchantPaymentId Id { get; }
        public string? AuthCode { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string MerchantRef { get; }
        public int Amount { get; }
        public string Currency { get; }
        public string CardBin { get; }
        public string CardLast4Digits { get; }
        public string CardExpiry { get; }
        public string CardHolder { get; }

        public PaymentStatusRecord(MerchantPaymentId id, PaymentStatus paymentStatus, string merchantRef, int amount, string currency, string cardBin, string cardLast4Digits, string cardExpiry, string cardHolder, string? authCode = null)
        {
            Id = id;
            PaymentStatus = paymentStatus;
            MerchantRef = merchantRef;
            Amount = amount;
            Currency = currency;
            CardBin = cardBin;
            CardLast4Digits = cardLast4Digits;
            CardExpiry = cardExpiry;
            CardHolder = cardHolder;
            AuthCode = authCode;
        }

    }
}