using System;
using System.Reflection;
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
        public async Task SaveAsync(T aggregate, long expectedVersion)
        {
            await _eventStore.SaveEventsAsync(aggregate.Id, aggregate.GetUncommittedChanges(), expectedVersion);

            aggregate.ClearUncommittedEvents();
        }

        public async Task<T> GetByIdAsync(TId id)
        {
            var obj = CreateAggregate();

            var events = await _eventStore.GetEventsForAggregateAsync(id);

            obj.LoadsFromHistory(events);

            return obj;
        }

        private static T CreateAggregate()
        {
            var ctor = typeof(T).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                null, Type.EmptyTypes, Array.Empty<ParameterModifier>());

            if (ctor == null)
            {
                throw new InvalidOperationException($"No parameterless constructor found for {typeof(T).FullName}");
            }

            return (T) ctor.Invoke(Array.Empty<object>());
        }
    }
}