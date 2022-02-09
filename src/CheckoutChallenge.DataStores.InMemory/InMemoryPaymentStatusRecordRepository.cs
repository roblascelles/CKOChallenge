using System.Collections.Generic;
using System.Threading.Tasks;
using CheckoutChallenge.Application.Domain;
using CheckoutChallenge.Application.PaymentProcessing;
using CheckoutChallenge.Application.PaymentRetrieval;

namespace CheckoutChallenge.DataStores.InMemory
{
    public class InMemoryPaymentStatusRecordRepository : IPaymentStatusRecordRepository
    {
        private readonly Dictionary<MerchantPaymentId, PaymentStatusRecord> _records = new Dictionary<MerchantPaymentId, PaymentStatusRecord>();

        public Task SaveAsync(MerchantPaymentId id, PaymentStatusRecord record)
        {
            _records[id] = record;
            return Task.CompletedTask;
        }

        public Task<PaymentStatusRecord?> GetByIdAsync(MerchantPaymentId id)
        {
            var record = _records.ContainsKey(id) ? _records[id] : null;
            return Task.FromResult(record);
        }

        public Task UpdateAuthStatusAsync(MerchantPaymentId id, PaymentStatus status, string? authCode = null)
        {
            if (_records.ContainsKey(id))
            {
                _records[id].PaymentStatus = status;
                _records[id].AuthCode = authCode;
            }
            return Task.CompletedTask;
        }
    }
}