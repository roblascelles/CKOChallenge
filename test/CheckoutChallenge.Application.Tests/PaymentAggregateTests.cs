using System.Threading.Tasks;
using CheckoutChallenge.Application.Acquirers;
using CheckoutChallenge.Application.Bus;
using CheckoutChallenge.Application.DataStore;
using CheckoutChallenge.Application.Domain;
using CheckoutChallenge.Application.PaymentProcessing;
using CheckoutChallenge.DataStores.InMemory;
using FakeItEasy;
using Shouldly;
using Xunit;

namespace CheckoutChallenge.Application.Tests;

public class PaymentAggregateTests
{
    private readonly IAcquirer _acquirer = A.Fake<IAcquirer>();
    private readonly IEventStore<MerchantPaymentId> _eventStore = new InMemoryEventStore<MerchantPaymentId>(A.Fake<IEventPublisher>());

    private readonly Repository<PaymentAggregate, MerchantPaymentId> _repository;

    public PaymentAggregateTests()
    {
        _repository = new Repository<PaymentAggregate, MerchantPaymentId>(_eventStore);
    }

    [Fact]
    public async Task CanSaveAndRestoreNewPayment()
    {
        var id = MerchantPaymentId.CreateNew("MerchantABC");
        var card = new Card("2030/01", "111", "4111111111111111", "Test Holder");

        var aggregate = new PaymentAggregate(id, "shop-ref", 1367, "GBP", card);

        await _repository.SaveAsync(aggregate, AggregateRoot.NewVersion);


        var restoredAggregate = await _repository.GetByIdAsync(id);
        
        restoredAggregate.Id.ShouldBe(id);
        restoredAggregate.Version.ShouldBe(0);

        restoredAggregate.MerchantRef.ShouldBe("shop-ref");
        restoredAggregate.Currency.ShouldBe("GBP");
        restoredAggregate.Amount.ShouldBe(1367);

        restoredAggregate.Card.CardHolderName.ShouldBe("Test Holder");
        restoredAggregate.Card.Expiry.ShouldBe("2030/01");
        restoredAggregate.Card.Cvv.ShouldBe("111");
        restoredAggregate.Card.Pan.ShouldBe("4111111111111111");
    }


    [Fact]
    public async Task CanSaveAndRestoreAndAuthoriseNewPayment()
    {
        var id = MerchantPaymentId.CreateNew("MerchantABC");
        var card = new Card("2030/01", "111", "4111111111111111", "Test Holder");
        var aggregate = new PaymentAggregate(id, "shop-ref", 1367, "GBP", card);

        await _repository.SaveAsync(aggregate, AggregateRoot.NewVersion);

        var restoredAggregate = await _repository.GetByIdAsync(id);

        restoredAggregate.Id.ShouldBe(id);
        restoredAggregate.Version.ShouldBe(0);

        SetUpFakeAcquirer(AuthorisationStatus.Authorised);

        var response = await restoredAggregate.Authorise(_acquirer);
        response.Status.ShouldBe(PaymentStatus.Authorized);

        await _repository.SaveAsync(restoredAggregate, 0);

        restoredAggregate = await _repository.GetByIdAsync(id);
        restoredAggregate.Version.ShouldBe(1);

    }

    private void SetUpFakeAcquirer(AuthorisationStatus acquirerStatus)
    {
        A.CallTo(() => _acquirer.AuthoriseAsync(A<AuthorisationRequest>._)).ReturnsLazily((p) =>
        {
            var request = p.Arguments[0] as AuthorisationRequest;
            request.ShouldNotBeNull();

            return Task.FromResult(new AuthorisationResponse(request.Amount, request.Currency, acquirerStatus, "acquirer-auth-code"));
        });
    }
}