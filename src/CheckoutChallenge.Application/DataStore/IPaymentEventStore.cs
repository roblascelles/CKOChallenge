using System.Collections.Generic;
using System.Threading.Tasks;
using CheckoutChallenge.Application.Domain;

namespace CheckoutChallenge.Application.DataStore
{
    public interface IPaymentEventStore
    {
        Task SaveEventsAsync(MerchantPaymentId id, IEnumerable<MerchantPaymentEvent> events);
    }
}