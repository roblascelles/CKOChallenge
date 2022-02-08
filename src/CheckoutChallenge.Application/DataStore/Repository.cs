using System.Threading.Tasks;
using CheckoutChallenge.Application.Domain;

namespace CheckoutChallenge.Application.DataStore
{
    public class Repository<T, TId> : IRepository<T, TId> where T : AggregateRoot<TId>, new()
    {
        private readonly IEventStore<TId> _eventStore;

        public Repository(IEventStore<TId> eventStore)
        {
            _eventStore = eventStore;
        }
        public async Task SaveAsync(T aggregate, long expectedVersion)
        {
            await _eventStore.SaveEventsAsync(aggregate.Id, aggregate.GetUncommittedChanges(), expectedVersion);

            aggregate.ClearUncommittedEvents();
        }

        public async Task<T> GetByIdAsync(TId id)
        {
            var obj = new T();

            var events = await _eventStore.GetEventsForAggregateAsync(id);

            obj.LoadsFromHistory(events);

            return obj;
        }
    }
}