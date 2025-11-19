using Catalog.API.Common;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using Mapster;
using MediatR;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("CatalogDb");

builder.Services.AddDbContext<Catalog.API.Infrastructure.Data.CatalogContext>(options =>
    options.UseNpgsql(connectionString));

var assembly = typeof(Program).Assembly;

builder.Services.AddMediatR(config =>
    config.RegisterServicesFromAssembly(assembly));
builder.Services.AddValidatorsFromAssembly(assembly);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(Catalog.API.Common.Behaviors.ValidationBehavior<,>));
builder.Services.AddMapster();

builder.Services.AddEndpointDefinitions(typeof(Program));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseEndpointDefinitions();

app.Run();
