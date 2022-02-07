namespace CheckoutChallenge.Application.Domain
{
    public class PaymentAuthorised : MerchantPaymentEvent
    {
        public PaymentAuthorised(MerchantPaymentId id, int amount, string currency) : base(id)
        {
            Amount = amount;
            Currency = currency;
        }
        public int Amount { get; }
        public string Currency { get; }

    }
}