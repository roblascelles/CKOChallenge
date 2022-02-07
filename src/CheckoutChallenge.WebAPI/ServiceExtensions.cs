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
        services.AddSingleton<IPaymentStatusRecordRepository, InMemoryPaymentStatusRecordRepository>();
        
        services.AddSingleton<IPaymentRepository, PaymentRepository>();
        services.AddSingleton<IPaymentEventStore, InMemoryPaymentEventStore>();

        services.AddSingleton<IAcquirer, FakeAcquirer>();

        services.AddBus();

        services.AddScoped<PaymentQueryHandler>();
        services.AddScoped<ProcessPaymentHandler>();

        return services;
    }

    public static IServiceCollection AddBus(this IServiceCollection services)
    {
        var bus = new InProcessBus();

        services.AddSingleton<IPaymentEventPublisher>(bus);
        services.AddSingleton<IPaymentEventSubscriber>(bus);

        var statusRecordRepository = new InMemoryPaymentStatusRecordRepository();
        services.AddSingleton<IPaymentStatusRecordRepository>(statusRecordRepository);

        var view = new PaymentStatusView(statusRecordRepository);

        view.Subscribe(bus);

        return services;
    }
}