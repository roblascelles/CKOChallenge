using System.Collections.Generic;
using CheckoutChallenge.Application.Domain.Events;

namespace CheckoutChallenge.Application.Domain
{
    public abstract class AggregateRoot<TId>
    {
        public TId Id { get; }
        private readonly List<Event<TId>> _changes = new List<Event<TId>>();

        public AggregateRoot(TId id)
        {
            Id = id;
        }

        public IEnumerable<Event<TId>> GetUncommittedChanges()
        {
            return _changes;
        }
        protected void ApplyChange(Event<TId> @event)
        {
            _changes.Add(@event);
        }
    }
}