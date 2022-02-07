namespace CheckoutChallenge.Application.PaymentRetrieval
{
    public class PaymentQuery
    {
        public PaymentQuery(string merchantId, string paymentId)
        {
            MerchantId = merchantId;
            PaymentId = paymentId;
        }

        public string MerchantId { get; }
        public string PaymentId { get; }
    }
}
