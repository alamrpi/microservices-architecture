using HealthChecks.UI.Client;
using MassTransit;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Ordering.API.Exceptions;
using Ordering.Application.Abstractions;
using Ordering.Application.Orders.Commands.CreateOrder;
using Ordering.Application.Orders.EventHandlers;
using Ordering.Infrastructure.Persistence;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<OrderingContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("OrderingDb")));

builder.Services.AddScoped<IOrderingDbContext, OrderingContext>();

builder.Services.AddMediatR(config =>
    config.RegisterServicesFromAssembly(typeof(CreateOrderCommand).Assembly));

builder.Services.AddMassTransit(config =>
{
    config.AddConsumer<BasketCheckoutConsumer>();

    config.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint("basket-checkout-queue", c =>
        {
            c.ConfigureConsumer<BasketCheckoutConsumer>(context);
        });
    });
});

// --- 4. API & Swagger Setup ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy())
    .AddSqlServer(builder.Configuration.GetConnectionString("OrderingDb")!);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<OrderingContext>();
        context.Database.Migrate();
    }
}
app.UseExceptionHandler();

app.UseHttpsRedirection();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();

