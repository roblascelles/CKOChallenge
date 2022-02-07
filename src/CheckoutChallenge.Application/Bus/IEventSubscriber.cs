using System;
using System.Threading.Tasks;
using CheckoutChallenge.Application.Domain;
using CheckoutChallenge.Application.Domain.Events;

namespace CheckoutChallenge.Application.Bus
{
    public interface IEventSubscriber
    {
        void Subscribe<T>(Func<T, Task> handler) where T : Event;
    }
}