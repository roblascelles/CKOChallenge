using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace CheckoutChallenge.WebAPI.Tests;

internal class TestWebAPIHost : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.AddCheckoutChallengeServices();
        });

        return base.CreateHost(builder);
    }
}