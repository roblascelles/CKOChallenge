namespace CheckoutChallenge.Application.Domain.Events
{
    public class PaymentAuthorised : Event<MerchantPaymentId>
    {
        public PaymentAuthorised(MerchantPaymentId id, int amount, string currency, string? authCode) : base(id)
        {
            Amount = amount;
            Currency = currency;
            AuthCode = authCode;
        }
        public int Amount { get; }
        public string Currency { get; }
        public string? AuthCode { get; }
    }
}