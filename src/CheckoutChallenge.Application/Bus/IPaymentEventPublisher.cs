using System.Threading.Tasks;
using CheckoutChallenge.Application.Domain;

namespace CheckoutChallenge.Application.Bus
{
    public interface IPaymentEventPublisher
    {
        Task PublishAsync<T>(T @event) where T : MerchantPaymentEvent;
    }
}
