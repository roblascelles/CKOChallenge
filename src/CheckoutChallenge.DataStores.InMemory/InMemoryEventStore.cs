using System.Collections.Generic;
using System.Threading.Tasks;
using CheckoutChallenge.Application.Bus;
using CheckoutChallenge.Application.DataStore;
using CheckoutChallenge.Application.Domain.Events;

namespace CheckoutChallenge.DataStores.InMemory
{
    public class InMemoryEventStore<TId> : IEventStore<TId>
    {
        private readonly IEventPublisher _eventPublisher;

        public InMemoryEventStore(IEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
        }

        public async Task SaveEventsAsync(TId id, IEnumerable<Event<TId>> events)
        {
            foreach (var @event in events)
            {
                await _eventPublisher.PublishAsync(@event);
            }
        }
    }
}