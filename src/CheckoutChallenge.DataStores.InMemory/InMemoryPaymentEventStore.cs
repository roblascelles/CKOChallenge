using System.Collections.Generic;
using System.Threading.Tasks;
using CheckoutChallenge.Application.Bus;
using CheckoutChallenge.Application.DataStore;
using CheckoutChallenge.Application.Domain;

namespace CheckoutChallenge.DataStores.InMemory
{
    public class InMemoryPaymentEventStore : IPaymentEventStore
    {
        private readonly IPaymentEventPublisher _eventPublisher;

        public InMemoryPaymentEventStore(IPaymentEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
        }

        public async Task SaveEventsAsync(MerchantPaymentId id, IEnumerable<MerchantPaymentEvent> events)
        {
            foreach (var @event in events)
            {
                await _eventPublisher.PublishAsync(@event);
            }
        }
    }
}