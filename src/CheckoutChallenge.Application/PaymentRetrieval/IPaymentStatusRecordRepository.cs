using System.Threading.Tasks;
using CheckoutChallenge.Application.Domain;
using CheckoutChallenge.Application.PaymentProcessing;

namespace CheckoutChallenge.Application.PaymentRetrieval
{
    public interface IPaymentStatusRecordRepository
    {
        Task SaveAsync(MerchantPaymentId id, PaymentStatusRecord record);
        Task<PaymentStatusRecord?> GetByIdAsync(MerchantPaymentId id);
        Task UpdateAuthStatusAsync(MerchantPaymentId id, PaymentStatus status, string? authCode = null);
    }
}