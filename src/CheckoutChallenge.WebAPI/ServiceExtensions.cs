using CheckoutChallenge.Acquirers.Faked;
using CheckoutChallenge.Application.Acquirers;
using CheckoutChallenge.Application.Bus;
using CheckoutChallenge.Application.DataStore;
using CheckoutChallenge.Application.PaymentProcessing;
using CheckoutChallenge.Application.PaymentRetrieval;
using CheckoutChallenge.DataStores.InMemory;

namespace CheckoutChallenge.WebAPI;

public static class ServiceExtensions
{
    public static IServiceCollection AddCheckoutChallengeServices(this IServiceCollection services)
    {
        services.AddSingleton<IPaymentRepository, InMemoryPaymentRepository>();
        services.AddSingleton<IAcquirer, FakeAcquirer>();
        services.AddSingleton<IPaymentEventPublisher, InProcessEventPublisher>();

        services.AddScoped<PaymentQueryHandler>();
        services.AddScoped<ProcessPaymentHandler>();

        return services;
    }

}