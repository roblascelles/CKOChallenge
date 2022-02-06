using System.Threading.Tasks;

namespace CheckoutChallenge.Application.DataStore
{
    public interface IPaymentRepository
    {
        Task SaveAsync(string merchantId, PaymentRecord record);

        Task<PaymentRecord?> GetByIdAsync(string merchantId, string paymentId);
    }
}
