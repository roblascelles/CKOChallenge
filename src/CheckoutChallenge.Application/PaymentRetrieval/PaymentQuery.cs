namespace CheckoutChallenge.Application.PaymentRetrieval
{
    public class PaymentQuery
    {
        public PaymentQuery(string merchantId, string id)
        {
            MerchantId = merchantId;
            Id = id;
        }

        public string MerchantId { get; }
        public string Id { get; }
    }
}
