namespace CheckoutChallenge.Application.Domain
{
    public class PaymentCreated : MerchantPaymentEvent
    {
        public PaymentCreated(MerchantPaymentId id, string merchantRef, int amount, string currency, string expiry, string cvv, string pan, string cardHolderName) : base(id)
        {
            MerchantRef = merchantRef;
            Amount = amount;
            Currency = currency;
            Expiry = expiry;
            Cvv = cvv;
            Pan = pan;
            CardHolderName = cardHolderName;
        }
        public string MerchantRef { get; }
        public int Amount { get; }
        public string Currency { get; }
        public string Expiry { get; }
        public string Cvv { get; }
        public string Pan { get; }
        public string CardHolderName { get; }

    }
}
