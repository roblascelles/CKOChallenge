using System.Threading.Tasks;
using CheckoutChallenge.Application.Domain;

namespace CheckoutChallenge.Application.DataStore
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly IPaymentEventStore _eventStore;

        public PaymentRepository(IPaymentEventStore eventStore)
        {
            _eventStore = eventStore;
        }
        public async Task SaveAsync(PaymentAggregate payment)
        {
            await _eventStore.SaveEventsAsync(payment.Id, payment.GetUncommittedChanges());
        }
    }
}