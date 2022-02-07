namespace CheckoutChallenge.Application.Domain
{
    public class MerchantPaymentEvent
    {
        public MerchantPaymentEvent(MerchantPaymentId id)
        {
            Id = id;
        }
        public MerchantPaymentId Id { get; }
    }
}