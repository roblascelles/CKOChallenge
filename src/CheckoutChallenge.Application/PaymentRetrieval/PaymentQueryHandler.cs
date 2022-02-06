using System.Threading.Tasks;
using CheckoutChallenge.Application.DataStore;

namespace CheckoutChallenge.Application.PaymentRetrieval
{
    public class PaymentQueryHandler
    {
        private readonly IPaymentRepository _repository;

        public PaymentQueryHandler(IPaymentRepository repository)
        {
            _repository = repository;
        }
        public async Task<PaymentQueryResponse?> HandleAsync(PaymentQuery query)
        {
            var record = await _repository.GetByIdAsync(query.MerchantId, query.Id);
            return record == null? null : new PaymentQueryResponse(record.Id, record.Status, record.AuthCode);
        }
    }
}