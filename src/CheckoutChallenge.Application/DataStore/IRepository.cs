using System;
using System.Threading.Tasks;
using CheckoutChallenge.Application.Domain;

namespace CheckoutChallenge.Application.DataStore
{
    public interface IRepository<T, TId> where T : AggregateRoot<TId>
    {
        Task SaveAsync(T aggregate, long expectedVersion);
        Task<T> GetByIdAsync(TId id);
    }
}
