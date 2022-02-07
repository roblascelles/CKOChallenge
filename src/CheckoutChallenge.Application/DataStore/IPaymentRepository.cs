using System;
using System.Threading.Tasks;
using CheckoutChallenge.Application.Domain;

namespace CheckoutChallenge.Application.DataStore
{
    public interface IPaymentRepository
    {
        Task SaveAsync(PaymentAggregate payment);
    }
}
