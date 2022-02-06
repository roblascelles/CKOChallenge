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

        public Task<PaymentRecord?> GetByIdAsync(string merchantId, string paymentId)
        {
            var key = Key(merchantId, paymentId);
            var record = _records.ContainsKey(key) ? _records[key] : null;
            return Task.FromResult(record);
        }

        private static string Key(string merchantId, string paymentId)
        {
            return $"{merchantId};{paymentId}";
        }
    }
}
