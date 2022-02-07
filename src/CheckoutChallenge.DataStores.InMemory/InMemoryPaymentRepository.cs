using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CheckoutChallenge.Application.DataStore;

namespace CheckoutChallenge.DataStores.InMemory
{
    public class InMemoryPaymentRepository : IPaymentRepository
    {
        private readonly Dictionary<string, PaymentRecord> _records = new Dictionary<string, PaymentRecord>();

        public Task SaveAsync(string merchantId, PaymentRecord record)
        {
            _records[Key(merchantId, record.Id)] = record;
            return Task.CompletedTask;
        }

        private static string Key(string merchantId, string paymentId)
        {
            return $"{merchantId};{paymentId}";
        }
    }
}
