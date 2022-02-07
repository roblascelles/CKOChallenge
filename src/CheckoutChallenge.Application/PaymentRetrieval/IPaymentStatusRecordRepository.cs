using System.Threading.Tasks;
using CheckoutChallenge.Application.Domain;

namespace CheckoutChallenge.Application.PaymentRetrieval
{
    public interface IPaymentStatusRecordRepository
    {
        Task SaveAsync(MerchantPaymentId id, PaymentStatusRecord record);
        Task<PaymentStatusRecord?> GetByIdAsync(MerchantPaymentId id);
    }
}