namespace CheckoutChallenge.Application.Domain
{
    public class PaymentAuthorised : MerchantPaymentEvent
    {
        public PaymentAuthorised(MerchantPaymentId id, int amount, string currency, string authCode) : base(id)
        {
            Amount = amount;
            Currency = currency;
            AuthCode = authCode;
        }
        public int Amount { get; }
        public string Currency { get; }
        public string AuthCode { get; }
    }
}