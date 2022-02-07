using System;
using System.Threading.Tasks;
using CheckoutChallenge.Application.Domain;

namespace CheckoutChallenge.Application.Bus
{
    public interface IPaymentEventSubscriber
    {
        void Subscribe<T>(Func<T, Task> handler) where T : MerchantPaymentEvent;
    }
}