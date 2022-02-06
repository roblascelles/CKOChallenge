using CheckoutChallenge.Application.PaymentProcessing;
using CheckoutChallenge.Application.PaymentRetrieval;
using CheckoutChallenge.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CheckoutChallenge.WebAPI.Controllers
{
    [Route("api/payments")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly PaymentQueryHandler _queryHandler;
        private readonly ProcessPaymentHandler _processPaymentHandler;

        public PaymentController(PaymentQueryHandler queryHandler, ProcessPaymentHandler processPaymentHandler)
        {
            _queryHandler = queryHandler;
            _processPaymentHandler = processPaymentHandler;
        }

        [HttpGet("{merchantId}/{paymentId}")]
        public async Task<ActionResult<PaymentStatusResponse>> Get(string merchantId, string paymentId)
        {

            var response = await _queryHandler.HandleAsync(new PaymentQuery(merchantId, paymentId));
            if (response == null)
            {
                return NotFound();
            }

            return response.ToApiResponse();
        }

        [HttpPost]
        public async Task<ActionResult<ProcessPaymentResponse>> Post([FromBody] ProcessPaymentRequest request)
        {
            var response = await _processPaymentHandler.HandleAsync(request.ToCommand());
            return response.ToApiResponse();

        }
    }
}
