using CheckoutChallenge.Acquirers.Faked;
using CheckoutChallenge.Application.Acquirers;
using CheckoutChallenge.Application.Bus;
using CheckoutChallenge.Application.DataStore;
using CheckoutChallenge.Application.Domain;
using CheckoutChallenge.Application.PaymentProcessing;
using CheckoutChallenge.Application.PaymentRetrieval;
using CheckoutChallenge.DataStores.InMemory;
using CheckoutChallenge.WebAPI.Auth;

namespace CheckoutChallenge.WebAPI;

public static class ServiceExtensions
{
    public static IServiceCollection AddCheckoutChallengeServices(this IServiceCollection services)
    {
        services.AddSingleton<IPaymentStatusRecordRepository, InMemoryPaymentStatusRecordRepository>();
        
        services.AddSingleton<IRepository<PaymentAggregate, MerchantPaymentId>, Repository<PaymentAggregate, MerchantPaymentId>>();
        services.AddSingleton<IEventStore<MerchantPaymentId>, InMemoryEventStore<MerchantPaymentId>>();

        services.AddSingleton<IAcquirer, FakeAcquirer>();

        services.AddSingleton<IApiKeyStore, TestAPIKeyStore>();

        services.AddBus();

        services.AddScoped<PaymentQueryHandler>();
        services.AddScoped<ProcessPaymentHandler>();

        return services;
    }

    public static IServiceCollection AddBus(this IServiceCollection services)
    {
        var bus = new InProcessBus();

        services.AddSingleton<IEventPublisher>(bus);
        services.AddSingleton<IEventSubscriber>(bus);

        var statusRecordRepository = new InMemoryPaymentStatusRecordRepository();
        services.AddSingleton<IPaymentStatusRecordRepository>(statusRecordRepository);

        var view = new PaymentStatusView(statusRecordRepository);

        view.Subscribe(bus);

        return services;
    }
}