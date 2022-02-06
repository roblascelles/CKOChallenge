using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CheckoutChallenge.WebAPI.Models;
using Shouldly;
using Xunit;

namespace CheckoutChallenge.WebAPI.Tests
{
    public class ValidationTests
    {
        [Fact]
        public async Task NotFoundPaymentStatusReturns404()
        {
            await using var application = new TestWebAPIHost();

            var merchantId = "unknown";
            var paymentId = "unknown";

            var client = application.CreateClient();

            var response = await client.GetAsync($"/api/payments/{merchantId}/{paymentId}");
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Theory]
        [InlineData("", "ref", 1299, "GBP", "2030-12", "123", "4111111111111111", "Mrs Test")]
        [InlineData("merchant", "", 1299, "GBP", "2030-12", "123", "4111111111111111", "Mrs Test")]
        [InlineData("merchant", "ref", -1, "GBP", "2030-12", "123", "4111111111111111", "Mrs Test")]
        [InlineData("merchant", "ref", 1299, "GBPP", "2030-12", "123", "4111111111111111", "Mrs Test")]
        [InlineData("merchant", "ref", 1299, "GBP", "30-12", "123", "4111111111111111", "Mrs Test")]
        [InlineData("merchant", "ref", 1299, "GBP", "2030-12", "12345", "4111111111111111", "Mrs Test")]
        [InlineData("merchant", "ref", 1299, "GBP", "2030-12", "123", "X4111111111111111", "Mrs Test")]
        [InlineData("merchant", "ref", 1299, "GBP", "2030-12", "123", "4111111111111111", "")]
        public async Task InvalidProcessPaymentRequestsReturn400s(string merchantId, string merchantRef, int amount, string currency, string expiry, string cvv, string pan, string cardholderName)
        {
            await using var application = new TestWebAPIHost();

            var client = application.CreateClient();

            var request = new ProcessPaymentRequest()
            {
                MerchantId = merchantId,
                MerchantRef = merchantRef,
                Amount = amount,
                CardHolderName = cardholderName,
                Currency = currency,
                Cvv = cvv,
                Expiry = expiry,
                Pan = pan
            };

            var response = await client.PostAsJsonAsync($"/api/payments", request);
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

  
    }
}