using System.Threading.Tasks;
using CheckoutChallenge.Application.Domain;

namespace CheckoutChallenge.Application.PaymentRetrieval
{
    public class PaymentQueryHandler
    {
        private readonly IPaymentStatusRecordRepository _repository;

        public PaymentQueryHandler(IPaymentStatusRecordRepository repository)
        {
            _repository = repository;
        }
        public async Task<PaymentQueryResponse?> HandleAsync(PaymentQuery query)
        {
            var record = await _repository.GetByIdAsync(new MerchantPaymentId(query.MerchantId, query.PaymentId));
            return record == null? null : new PaymentQueryResponse(record.Id.PaymentId, record.PaymentStatus, record.AuthCode);
        }
    }
}