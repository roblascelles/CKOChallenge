using System;
using System.Threading.Tasks;
using CheckoutChallenge.Application.Acquirers;
using CheckoutChallenge.Application.Bus;
using CheckoutChallenge.Application.DataStore;
using CheckoutChallenge.Application.PaymentProcessing;
using CheckoutChallenge.Application.PaymentRetrieval;
using CheckoutChallenge.DataStores.InMemory;
using FakeItEasy;
using Shouldly;
using Xunit;

namespace CheckoutChallenge.Application.Tests;

public class RetrievalTests
{
    private const string MerchantId = "MERCHANTA";

    [Theory]
    [InlineData(AuthorisationStatus.Authorised)]
    [InlineData(AuthorisationStatus.Declined)]
    [InlineData(AuthorisationStatus.Error)]
    public async Task CanAuthoriseAndRetrievePayment(AuthorisationStatus acquirerStatus)
    {
        var dataStore = new InMemoryPaymentRepository();

        var paymentResponse = await ProcessPayment(acquirerStatus, dataStore);

        var queryHandler = new PaymentQueryHandler(dataStore);
        var queryResponse = await queryHandler.HandleAsync(new PaymentQuery(MerchantId, paymentResponse.Id));

        queryResponse.ShouldNotBeNull();
        queryResponse.Id.ShouldBe(paymentResponse.Id);
        queryResponse.Status.ShouldBe(paymentResponse.Status);
        queryResponse.AuthCode.ShouldBe(paymentResponse.AuthCode);
    }


    private static async Task<PaymentResponse> ProcessPayment(AuthorisationStatus acquirerStatus, IPaymentRepository repository)
    {
        var acquirer = A.Fake<IAcquirer>();

        A.CallTo(() => acquirer.AuthoriseAsync(A<AuthorisationRequest>._)).ReturnsLazily((p) =>
        {
            var request = p.Arguments[0] as AuthorisationRequest;
            request.ShouldNotBeNull();

            return Task.FromResult(new AuthorisationResponse(request.Amount, request.Currency, acquirerStatus, "acquirer-auth-code"));
        });

        var paymentHandler = new ProcessPaymentHandler(acquirer, repository, A.Fake<IPaymentEventPublisher>());
        var command = new ProcessPaymentCommand(MerchantId, "Order#1", 1299, "GBP", "2030/6", "737", "4111111111111111", "Mr Test");

        return await paymentHandler.HandleAsync(command);

    }
}