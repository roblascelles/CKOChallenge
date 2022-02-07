using System;

namespace CheckoutChallenge.Application.Domain
{
    public class MerchantPaymentId 
    {
        public MerchantPaymentId(string merchantId, string paymentId)
        {
            if (string.IsNullOrEmpty(merchantId)) throw new ArgumentNullException(nameof(merchantId));
            if (string.IsNullOrEmpty(paymentId)) throw new ArgumentNullException(nameof(paymentId));

            MerchantId = merchantId;
            PaymentId = paymentId;
        }

        public string MerchantId { get; }
        public string PaymentId { get; }

        public string AggregateId => $"{MerchantId};{PaymentId}";

        public override bool Equals(object obj)
        {
            return obj is MerchantPaymentId other && Equals(other);
        }

        protected bool Equals(MerchantPaymentId other)
        {
            return MerchantId == other.MerchantId && PaymentId == other.PaymentId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(MerchantId, PaymentId);
        }

        public override string ToString()
        {
            return AggregateId;
        }

        public static MerchantPaymentId CreateNew(string merchantId)
        {
            return new MerchantPaymentId(merchantId, Guid.NewGuid().ToString("N"));
        }
    }
}