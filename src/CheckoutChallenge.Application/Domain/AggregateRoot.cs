using System.Collections.Generic;
using CheckoutChallenge.Application.Domain.Events;

namespace CheckoutChallenge.Application.Domain
{
    public abstract class AggregateRoot<TId> : AggregateRoot
    {
        public TId Id { get; protected set; } = default!;

        private readonly List<Event<TId>> _changes = new List<Event<TId>>();

        public IEnumerable<Event<TId>> GetUncommittedChanges()
        {
            return _changes;
        }

        public void ClearUncommittedEvents()
        {
            _changes.Clear();
        }

        public void LoadsFromHistory(IEnumerable<Event<TId>> history)
        {
            foreach (var e in history)
            {
                ApplyChange(e, false);
                Version = e.Version;
            }
        }

        protected void ApplyChange(Event<TId> @event)
        {
            ApplyChange(@event, true);
        }

        protected void ApplyChange(Event<TId> @event, bool isNew)
        {
            ((dynamic) this).Apply((dynamic) @event);
            if (isNew) _changes.Add(@event);
        }
    }

    public abstract class AggregateRoot
    {
        public const long NewVersion = -1;
        public long Version { get; internal set; }

    }
}