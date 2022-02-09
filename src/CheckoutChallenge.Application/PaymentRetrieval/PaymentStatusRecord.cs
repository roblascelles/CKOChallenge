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
        public string CardNumber { get; }
        public string CardExpiry { get; }
        public string CardHolder { get; }

        public PaymentStatusRecord(MerchantPaymentId id, PaymentStatus paymentStatus, string merchantRef, int amount, string currency, string cardNumber, string cardExpiry, string cardHolder, string? authCode = null)
        {
            Id = id;
            PaymentStatus = paymentStatus;
            MerchantRef = merchantRef;
            Amount = amount;
            Currency = currency;
            CardNumber = cardNumber;
            CardExpiry = cardExpiry;
            CardHolder = cardHolder;
            AuthCode = authCode;
        }

    }
}