using System;
using System.Threading.Tasks;
using CheckoutChallenge.Application.Acquirers;
using CheckoutChallenge.Application.Bus;
using CheckoutChallenge.Application.DataStore;
using CheckoutChallenge.Application.Domain;
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

    private readonly IAcquirer _acquirer = A.Fake<IAcquirer>();
    private readonly InProcessBus _bus = new();
    private readonly IPaymentStatusRecordRepository _queryStore = new InMemoryPaymentStatusRecordRepository();

    private readonly IRepository<PaymentAggregate, MerchantPaymentId> _paymentRepository;

    public RetrievalTests()
    {
        var eventStore = new InMemoryEventStore<MerchantPaymentId>(_bus);
        _paymentRepository = new Repository<PaymentAggregate, MerchantPaymentId>(eventStore);
    }

    [Theory]
    [InlineData(AuthorisationStatus.Authorised)]
    public async Task CanAuthoriseAndRetrievePayment(AuthorisationStatus acquirerStatus)
    {
        var paymentResponse = await ProcessPayment(acquirerStatus);

        var queryHandler = new PaymentQueryHandler(_queryStore);
        var queryResponse = await queryHandler.HandleAsync(new PaymentQuery(MerchantId, paymentResponse.Id));

        queryResponse.ShouldNotBeNull();
        queryResponse.PaymentId.ShouldBe(paymentResponse.Id);
        queryResponse.Status.ShouldBe(paymentResponse.Status);

        queryResponse.AuthCode.ShouldBe(paymentResponse.AuthCode);

        queryResponse.Amount.ShouldBe(1299);
        queryResponse.Currency.ShouldBe("GBP");
        
        queryResponse.MerchantRef.ShouldBe("Order#1");
        
        queryResponse.CardExpiry.ShouldBe("2030/6");
        queryResponse.CardBin.ShouldBe("411111");
        queryResponse.CardLast4Digits.ShouldBe("1234");
        queryResponse.CardHolder.ShouldBe("Mr Test");
    }   
    
    [Theory]
    [InlineData(AuthorisationStatus.Declined)]
    [InlineData(AuthorisationStatus.Error)]
    public async Task CanFailToAuthoriseAndRetrievePayment(AuthorisationStatus acquirerStatus)
    {
        var paymentResponse = await ProcessPayment(acquirerStatus);

        var queryHandler = new PaymentQueryHandler(_queryStore);
        var queryResponse = await queryHandler.HandleAsync(new PaymentQuery(MerchantId, paymentResponse.Id));

        queryResponse.ShouldNotBeNull();
        queryResponse.PaymentId.ShouldBe(paymentResponse.Id);
        queryResponse.Status.ShouldBe(paymentResponse.Status);

        queryResponse.AuthCode.ShouldBeNull();

        queryResponse.Amount.ShouldBe(1299);
        queryResponse.Currency.ShouldBe("GBP");

        queryResponse.MerchantRef.ShouldBe("Order#1");

        queryResponse.CardExpiry.ShouldBe("2030/6");
        queryResponse.CardBin.ShouldBe("411111");
        queryResponse.CardLast4Digits.ShouldBe("1234");
        queryResponse.CardHolder.ShouldBe("Mr Test");
    }


    private async Task<PaymentResponse> ProcessPayment(AuthorisationStatus acquirerStatus)
    {
        A.CallTo(() => _acquirer.AuthoriseAsync(A<AuthorisationRequest>._)).ReturnsLazily((p) =>
        {
            var request = p.Arguments[0] as AuthorisationRequest;
            request.ShouldNotBeNull();

            return Task.FromResult(new AuthorisationResponse(request.Amount, request.Currency, acquirerStatus, "acquirer-auth-code"));
        });

        var view = new PaymentStatusView(_queryStore);
        view.Subscribe(_bus);

        var paymentHandler = new ProcessPaymentHandler(_acquirer, _paymentRepository);
        var command = new ProcessPaymentCommand(MerchantId, "Order#1", 1299, "GBP", "2030/6", "737", "4111111111111234", "Mr Test");

        return await paymentHandler.HandleAsync(command);

    }
}