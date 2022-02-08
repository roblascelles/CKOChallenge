using CheckoutChallenge.Application.PaymentProcessing;
using CheckoutChallenge.Application.PaymentRetrieval;
using CheckoutChallenge.WebAPI.Auth;
using CheckoutChallenge.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CheckoutChallenge.WebAPI.Controllers
{
    [Authorize]
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

        [HttpGet("{paymentId}")]
        public async Task<ActionResult<PaymentStatusResponse>> Get(string paymentId)
        {
            if (!HttpContext.User.TryGetMerchantId(out var merchantId))
            {
                return Unauthorized();
            }

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
            if (!HttpContext.User.TryGetMerchantId(out var merchantId))
            {
                return Unauthorized();
            }

            var response = await _processPaymentHandler.HandleAsync(request.ToCommand(merchantId));
            return response.ToApiResponse();

        }
    }
}
