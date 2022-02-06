using System.Text.Json.Serialization;
using CheckoutChallenge.Acquirers.Faked;
using CheckoutChallenge.Application.Acquirers;
using CheckoutChallenge.Application.DataStore;
using CheckoutChallenge.Application.PaymentProcessing;
using CheckoutChallenge.Application.PaymentRetrieval;
using CheckoutChallenge.DataStores.InMemory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddControllers()
    .AddJsonOptions(opts =>
    {
        var enumConverter = new JsonStringEnumConverter();
        opts.JsonSerializerOptions.Converters.Add(enumConverter);
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IPaymentRepository>(new InMemoryPaymentRepository());
builder.Services.AddSingleton<IAcquirer>(new FakeAcquirer());

builder.Services.AddScoped<PaymentQueryHandler>();
builder.Services.AddScoped<ProcessPaymentHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
