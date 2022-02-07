using System;

namespace CheckoutChallenge.Application.Domain.Events
{
    public abstract class Event<TId> : Event
    {
        public TId AggregateId { get; }
       
        protected Event(TId aggregateId) : base()
        {
            AggregateId = aggregateId;
        }
    }

    public abstract class Event
    {
        public Guid EventId { get; }

        protected Event()
        {
            EventId = Guid.NewGuid();
        }
    }
}