using CheckoutChallenge.Acquirers.Faked;
using CheckoutChallenge.Application.Acquirers;
using CheckoutChallenge.Application.DataStore;
using CheckoutChallenge.Application.PaymentProcessing;
using CheckoutChallenge.Application.PaymentRetrieval;
using CheckoutChallenge.DataStores.InMemory;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CheckoutChallenge.WebAPI.Tests;

internal class TestWebAPIHost : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.AddSingleton<IPaymentRepository>(new InMemoryPaymentRepository());
            services.AddSingleton<IAcquirer>(new FakeAcquirer());

            services.AddScoped<PaymentQueryHandler>();
            services.AddScoped<ProcessPaymentHandler>();
        });

        return base.CreateHost(builder);
    }
}