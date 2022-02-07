using System.Threading.Tasks;

namespace CheckoutChallenge.Application.DataStore
{
    public interface IPaymentRepository
    {
        Task SaveAsync(string merchantId, PaymentRecord record);
    }
}
