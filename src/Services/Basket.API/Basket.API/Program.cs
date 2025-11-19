using Marten;
using MediatR;
using FluentValidation;
using Basket.API.Common;
using JasperFx;
using Basket.API.Protos;

var builder = WebApplication.CreateBuilder(args);
var assembly = typeof(Program).Assembly;

builder.Services.AddGrpcClient<DiscountService.DiscountServiceClient>(options =>
{
    options.Address = new Uri("https://localhost:7286");
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(config =>
    config.RegisterServicesFromAssembly(assembly));
builder.Services.AddValidatorsFromAssembly(assembly);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(Basket.API.Common.Behaviors.ValidationBehavior<,>));


builder.Services.AddMarten(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("Database")!;
    options.Connection(connectionString);

    options.AutoCreateSchemaObjects = AutoCreate.All;
}).UseLightweightSessions();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

builder.Services.AddEndpointDefinitions(typeof(Program));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseEndpointDefinitions();

app.Run();
