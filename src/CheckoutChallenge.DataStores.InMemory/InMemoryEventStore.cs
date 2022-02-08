using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CheckoutChallenge.Application.Bus;
using CheckoutChallenge.Application.DataStore;
using CheckoutChallenge.Application.Domain;
using CheckoutChallenge.Application.Domain.Events;

namespace CheckoutChallenge.DataStores.InMemory
{
    public class InMemoryEventStore<TId> : IEventStore<TId>
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly Dictionary<TId, List<Event<TId>>> _storage = new Dictionary<TId, List<Event<TId>>>();

        public InMemoryEventStore(IEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
        }

        public async Task SaveEventsAsync(TId id, IEnumerable<Event<TId>> events, long expectedVersion)
        {
            if (!_storage.TryGetValue(id, out var storedEvents))
            {
                storedEvents = new List<Event<TId>>();
                _storage.Add(id, storedEvents);
            }
            else if (storedEvents[^1].Version != expectedVersion && expectedVersion != AggregateRoot.NewVersion)
            {
                throw new ConcurrencyException();
            }


            var i = expectedVersion;

            foreach (var @event in events)
            {
                @event.Version = ++i;
                storedEvents.Add(@event);

                await _eventPublisher.PublishAsync(@event);
            }
        }

        public Task<List<Event<TId>>> GetEventsForAggregateAsync(TId id)
        {
            if (!_storage.TryGetValue(id, out var storedEvents))
            {
                throw new AggregateNotFoundException();
            }

            return Task.FromResult(storedEvents);
        }
    }

}