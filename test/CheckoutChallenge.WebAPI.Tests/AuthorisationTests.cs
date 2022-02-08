using System;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace CheckoutChallenge.WebAPI.Tests
{
    public class AuthorisationTests
    {
        [Fact]
        public async Task AuthorisedPaymentStatusReturns404()
        {
            await using var application = new TestWebAPIHost();

            var paymentId = "unknown";

            var client = application.CreateClient();
            
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("secret_key_test_a");

            var response = await client.GetAsync($"/api/payments/{paymentId}");
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("invalid_secret_key")]
        public async Task UnauthorisedPaymentStatusReturns401(string? authHeader)
        {
            await using var application = new TestWebAPIHost();

            var paymentId = "unknown";

            var client = application.CreateClient();

            client.DefaultRequestHeaders.Authorization = authHeader == null ? null : new AuthenticationHeaderValue(authHeader);

            var response = await client.GetAsync($"/api/payments/{paymentId}");
            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

    }
}
