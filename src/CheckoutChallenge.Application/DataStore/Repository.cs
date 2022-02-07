using System.Threading.Tasks;
using CheckoutChallenge.Application.Domain;

namespace CheckoutChallenge.Application.DataStore
{
    public class Repository<T, TId> : IRepository<T, TId> where T : AggregateRoot<TId>
    {
        private readonly IEventStore<TId> _eventStore;

        public Repository(IEventStore<TId> eventStore)
        {
            _eventStore = eventStore;
        }
        public async Task SaveAsync(T aggregate)
        {
            await _eventStore.SaveEventsAsync(aggregate.Id, aggregate.GetUncommittedChanges());
        }
    }
}