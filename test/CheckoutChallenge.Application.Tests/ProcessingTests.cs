using System;
using CheckoutChallenge.Application.Acquirers;
using CheckoutChallenge.Application.PaymentProcessing;
using FakeItEasy;
using Shouldly;
using System.Threading.Tasks;
using CheckoutChallenge.Application.Bus;
using CheckoutChallenge.Application.DataStore;
using CheckoutChallenge.Application.Domain;
using CheckoutChallenge.Application.Domain.Events;
using CheckoutChallenge.DataStores.InMemory;
using Xunit;

namespace CheckoutChallenge.Application.Tests
{
    public class ProcessingTests
    {
        private readonly IAcquirer _acquirer = A.Fake<IAcquirer>();
        private readonly InProcessBus _eventPublisher = new();
        private readonly ProcessPaymentHandler _commandHandler;

        private PaymentCreated? _paymentCreated;
        private PaymentAuthorised? _paymentAuthorised;
        private PaymentDeclined? _paymentDeclined;

        public ProcessingTests()
        {
            _eventPublisher.Subscribe<PaymentCreated>(OnPaymentCreated);
            _eventPublisher.Subscribe<PaymentAuthorised>(OnPaymentAuthorised);
            _eventPublisher.Subscribe<PaymentDeclined>(OnPaymentDeclined);

            var eventStore = new InMemoryEventStore<MerchantPaymentId>(_eventPublisher);
            var repository = new Repository<PaymentAggregate, MerchantPaymentId>(eventStore);

            _commandHandler = new ProcessPaymentHandler(_acquirer, repository);
        }

        [Theory]
        [InlineData(AuthorisationStatus.Authorised, true, PaymentStatus.Authorized)]
        [InlineData(AuthorisationStatus.Declined, false, PaymentStatus.Declined)]
        [InlineData(AuthorisationStatus.Error, false, PaymentStatus.Error)]
        [InlineData(AuthorisationStatus.Unknown, false, PaymentStatus.Unknown)]
        public async Task CanProcessPayment(AuthorisationStatus acquirerStatus, bool expectedApproval, PaymentStatus expectedStatus)
        {
            SetUpFakeAcquirer(acquirerStatus);

            var command = new ProcessPaymentCommand("MERCHANTA", "Order#1", 1299, "GBP", "2030/6", "737", "4111111111111111", "Mr Test");

            var result = await _commandHandler.HandleAsync(command);

            result.MerchantRef.ShouldBe(command.MerchantRef);

            result.Approved.ShouldBe(expectedApproval);

            result.Status.ShouldBe(expectedStatus);
            result.Amount.ShouldBe(command.Amount);
            result.Currency.ShouldBe(command.Currency);

            result.AuthCode.ShouldBe("acquirer-auth-code");
        }

        [Fact]
        public async Task PaymentCreatedEventGetsPublished()
        {
            SetUpFakeAcquirer(AuthorisationStatus.Authorised);

            var command = new ProcessPaymentCommand("MERCHANTA", "Order#1", 1299, "GBP", "2030/6", "737", "4111111111111234", "Mr Test");

            var result = await _commandHandler.HandleAsync(command);
            result.Approved.ShouldBe(true);

            _paymentCreated.ShouldNotBeNull();

            _paymentCreated.AggregateId.MerchantId.ShouldBe(command.MerchantId);
            _paymentCreated.AggregateId.PaymentId.ShouldNotBeNullOrWhiteSpace();
            _paymentCreated.AggregateId.PaymentId.ShouldBe(result.Id);
            
            _paymentCreated.Amount.ShouldBe(command.Amount);
            _paymentCreated.Currency.ShouldBe(command.Currency);

            _paymentCreated.MerchantRef.ShouldBe(command.MerchantRef);

            _paymentCreated.Card.CardHolderName.ShouldBe(command.CardHolderName);
            _paymentCreated.Card.Expiry.ShouldBe(command.Expiry);
            _paymentCreated.Card.Bin.ShouldBe("411111");
            _paymentCreated.Card.Last4Digits.ShouldBe("1234");
        }

        [Fact]
        public async Task PaymentAuthorisedEventGetsPublished()
        {
            SetUpFakeAcquirer(AuthorisationStatus.Authorised);

            var command = new ProcessPaymentCommand("MERCHANTA", "Order#1", 1299, "GBP", "2030/6", "737", "4111111111111111", "Mr Test");

            var result = await _commandHandler.HandleAsync(command);
            result.Approved.ShouldBe(true);

            _paymentDeclined.ShouldBeNull();

            _paymentAuthorised.ShouldNotBeNull();

            _paymentAuthorised.AggregateId.MerchantId.ShouldBe(command.MerchantId);
            _paymentAuthorised.AggregateId.PaymentId.ShouldNotBeNullOrWhiteSpace();
            _paymentAuthorised.AggregateId.PaymentId.ShouldBe(result.Id);

            _paymentAuthorised.Currency.ShouldBe(command.Currency);
            _paymentAuthorised.Amount.ShouldBe(command.Amount);
            _paymentAuthorised.AuthCode.ShouldBe("acquirer-auth-code");
        }

        [Theory]
        [InlineData(AuthorisationStatus.Declined, PaymentStatus.Declined)]
        [InlineData(AuthorisationStatus.CardStolen, PaymentStatus.Declined)]
        [InlineData(AuthorisationStatus.InsufficientFunds, PaymentStatus.Declined)]
        [InlineData(AuthorisationStatus.Error, PaymentStatus.Error)]
        [InlineData(AuthorisationStatus.Unknown, PaymentStatus.Unknown)]
        public async Task PaymentDeclinedEventGetsPublished(AuthorisationStatus authorisationStatus, PaymentStatus paymentStatus)
        {
            SetUpFakeAcquirer(authorisationStatus);

            var command = new ProcessPaymentCommand("MERCHANTA", "Order#1", 1299, "GBP", "2030/6", "737", "4111111111111111", "Mr Test");

            var result = await _commandHandler.HandleAsync(command);
            result.Approved.ShouldBe(false);

            _paymentAuthorised.ShouldBeNull();

            _paymentDeclined.ShouldNotBeNull();

            _paymentDeclined.AggregateId.MerchantId.ShouldBe(command.MerchantId);
            _paymentDeclined.AggregateId.PaymentId.ShouldNotBeNullOrWhiteSpace();
            _paymentDeclined.AggregateId.PaymentId.ShouldBe(result.Id);

            _paymentDeclined.AuthorisationStatus.ShouldBe(authorisationStatus);
            _paymentDeclined.PaymentStatus.ShouldBe(paymentStatus);
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

        private Task OnPaymentCreated(PaymentCreated @event)
        {
            _paymentCreated = @event;

            return Task.CompletedTask;
        }
        private Task OnPaymentAuthorised(PaymentAuthorised @event)
        {
            _paymentAuthorised = @event;

            return Task.CompletedTask;
        }
        private Task OnPaymentDeclined(PaymentDeclined @event)
        {
            _paymentDeclined = @event;
            return Task.CompletedTask;
        }

    }
}