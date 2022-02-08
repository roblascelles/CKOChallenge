using System.Collections.Generic;
using System.Threading.Tasks;
using CheckoutChallenge.Application.Domain.Events;

namespace CheckoutChallenge.Application.DataStore
{
    public interface IEventStore<TId>
    {
        Task SaveEventsAsync(TId id, IEnumerable<Event<TId>> events, long expectedVersion);
        Task<List<Event<TId>>> GetEventsForAggregateAsync(TId id);
    }
}