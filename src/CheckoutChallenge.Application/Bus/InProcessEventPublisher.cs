using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CheckoutChallenge.Application.Domain;

namespace CheckoutChallenge.Application.Bus
{
    public class InProcessEventPublisher : IPaymentEventPublisher
    {
        private readonly Dictionary<Type, List<Func<MerchantPaymentEvent, Task>>> _routes = new Dictionary<Type, List<Func<MerchantPaymentEvent, Task>>>();

        public void RegisterHandler<T>(Func<T, Task> handler) where T : MerchantPaymentEvent
        {
            if (!_routes.TryGetValue(typeof(T), out var handlers))
            {
                handlers = new List<Func<MerchantPaymentEvent, Task>>();
                _routes.Add(typeof(T), handlers);
            }

            handlers.Add((x => handler((T)x)));
        }


        public async Task PublishAsync<T>(T @event) where T : MerchantPaymentEvent
        {
            if (!_routes.TryGetValue(@event.GetType(), out var handlers)) return;

            foreach (var handler in handlers)
            {
                await handler(@event);
            }
        }
    }
}