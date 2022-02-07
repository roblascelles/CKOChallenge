using System;
using CheckoutChallenge.Application.Acquirers;
using CheckoutChallenge.Application.PaymentProcessing;
using FakeItEasy;
using Shouldly;
using System.Threading.Tasks;
using CheckoutChallenge.Application.Bus;
using CheckoutChallenge.Application.DataStore;
using CheckoutChallenge.Application.Domain;
using Xunit;

namespace CheckoutChallenge.Application.Tests
{
    public class ProcessingTests
    {
        private readonly IAcquirer _acquirer = A.Fake<IAcquirer>();
        private readonly InProcessEventPublisher _eventPublisher = new InProcessEventPublisher();
        private readonly ProcessPaymentHandler _commandHandler;

        private PaymentCreated _paymentCreated;
        private PaymentAuthorised _paymentAuthorised;
        private PaymentDeclined _paymentDeclined;

        public ProcessingTests()
        {
            _eventPublisher.RegisterHandler<PaymentCreated>(OnPaymentCreated);
            _eventPublisher.RegisterHandler<PaymentAuthorised>(OnPaymentAuthorised);
            _eventPublisher.RegisterHandler<PaymentDeclined>(OnPaymentDeclined);

            _commandHandler = new ProcessPaymentHandler(_acquirer, A.Fake<IPaymentRepository>(), _eventPublisher);
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

            var command = new ProcessPaymentCommand("MERCHANTA", "Order#1", 1299, "GBP", "2030/6", "737", "4111111111111111", "Mr Test");

            var result = await _commandHandler.HandleAsync(command);
            result.Approved.ShouldBe(true);

            _paymentCreated.ShouldNotBeNull();

            _paymentCreated.Id.MerchantId.ShouldBe(command.MerchantId);
            _paymentCreated.Id.PaymentId.ShouldNotBeNullOrWhiteSpace();
            _paymentCreated.Id.PaymentId.ShouldBe(result.Id);
            
            _paymentCreated.Amount.ShouldBe(command.Amount);
            _paymentCreated.CardHolderName.ShouldBe(command.CardHolderName);
            _paymentCreated.Currency.ShouldBe(command.Currency);
            _paymentCreated.Cvv.ShouldBe(command.Cvv);
            _paymentCreated.Expiry.ShouldBe(command.Expiry);
            _paymentCreated.MerchantRef.ShouldBe(command.MerchantRef);
            _paymentCreated.Pan.ShouldBe(command.Pan);
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

            _paymentAuthorised.Id.MerchantId.ShouldBe(command.MerchantId);
            _paymentAuthorised.Id.PaymentId.ShouldNotBeNullOrWhiteSpace();
            _paymentAuthorised.Id.PaymentId.ShouldBe(result.Id);

            _paymentAuthorised.Currency.ShouldBe(command.Currency);
            _paymentAuthorised.Amount.ShouldBe(command.Amount);
        }

        [Fact]
        public async Task PaymentDeclinedEventGetsPublished()
        {
            SetUpFakeAcquirer(AuthorisationStatus.Declined);

            var command = new ProcessPaymentCommand("MERCHANTA", "Order#1", 1299, "GBP", "2030/6", "737", "4111111111111111", "Mr Test");

            var result = await _commandHandler.HandleAsync(command);
            result.Approved.ShouldBe(false);

            _paymentAuthorised.ShouldBeNull();

            _paymentDeclined.ShouldNotBeNull();

            _paymentDeclined.Id.MerchantId.ShouldBe(command.MerchantId);
            _paymentDeclined.Id.PaymentId.ShouldNotBeNullOrWhiteSpace();
            _paymentDeclined.Id.PaymentId.ShouldBe(result.Id);

            _paymentDeclined.Status.ShouldBe(AuthorisationStatus.Declined);
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