using CheckoutChallenge.Application.PaymentProcessing;
using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace CheckoutChallenge.Application.Tests
{
    public class ProcessingTests
    {
        [Fact]
        public async Task CanProcessPayment()
        {
            var handler = new ProcessPaymentHandler();
            var command = new ProcessPaymentCommand("MERCHANTA", "Order#1", 1299, "GBP", "2030/6", "737", "4111111111111111", "Mr Test");

            var result = await handler.Handle(command);

            result.MerchantRef.ShouldBe(command.MerhantRef);

            result.Approved.ShouldBeTrue();

            result.Status.ShouldBe(PaymentStatus.Authorized);
            result.Amount.ShouldBe(command.Amount);
            result.Currency.ShouldBe(command.Currency);
        }
    }
}