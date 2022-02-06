using CheckoutChallenge.Application.Acquirers;
using CheckoutChallenge.Application.PaymentProcessing;
using FakeItEasy;
using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace CheckoutChallenge.Application.Tests
{
    public class ProcessingTests
    {
        [Theory]
        [InlineData(AuthorisationStatus.Authorised, true, PaymentStatus.Authorized)]
        [InlineData(AuthorisationStatus.Declined, false, PaymentStatus.Declined)]
        [InlineData(AuthorisationStatus.Error, false, PaymentStatus.Error)]
        [InlineData(AuthorisationStatus.Unknown, false, PaymentStatus.Unknown)]
        public async Task CanProcessPayment(AuthorisationStatus acquirerStatus, bool expectedApproval, PaymentStatus expectedStatus)
        {
            var acquirer = A.Fake<IAcquirer>();

            A.CallTo(() => acquirer.AuthoriseAsync(A<AuthorisationRequest>._)).ReturnsLazily((p) =>
            {
                var request = p.Arguments[0] as AuthorisationRequest;

                return Task.FromResult(new AuthorisationResponse(request.Amount, request.Currency, acquirerStatus, "acquirer-auth-code"));
            });

            var handler = new ProcessPaymentHandler(acquirer);
            var command = new ProcessPaymentCommand("MERCHANTA", "Order#1", 1299, "GBP", "2030/6", "737", "4111111111111111", "Mr Test");

            var result = await handler.Handle(command);

            result.MerchantRef.ShouldBe(command.MerhantRef);

            result.Approved.ShouldBe(expectedApproval);

            result.Status.ShouldBe(expectedStatus);
            result.Amount.ShouldBe(command.Amount);
            result.Currency.ShouldBe(command.Currency);

            result.AuthCode.ShouldBe("acquirer-auth-code");
        }
    }
}