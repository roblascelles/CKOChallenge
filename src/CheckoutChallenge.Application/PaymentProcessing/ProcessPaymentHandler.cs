using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CheckoutChallenge.Application.PaymentProcessing
{
    public class ProcessPaymentHandler
    {
        public Task<PaymentResponse> Handle(ProcessPaymentCommand command)
        {
            return Task.FromResult(new PaymentResponse(true, command.MerhantRef, PaymentStatus.Authorized, command.Amount, command.Currency, "ACQUIRER-AUTH-CODE"));
        }
    }
}
