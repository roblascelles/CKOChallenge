using System.Threading.Tasks;
using CheckoutChallenge.Application.Domain;
using CheckoutChallenge.Application.Domain.Events;

namespace CheckoutChallenge.Application.Bus
{
    public interface IEventPublisher
    {
        Task PublishAsync<T>(T @event) where T : Event;
    }
}
