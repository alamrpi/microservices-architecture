using Basket.API.Common;
using Basket.API.Core.Entities;
using Basket.API.Features.CheckoutBasket;
using Basket.API.Features.DeleteBasket;
using Basket.API.Features.GetBasket;
using Basket.API.Features.StoreBasket;
using Mapster;
using MediatR;

namespace Basket.API.Features;

public class BasketEndpoints : IEndpointDefinition
{
    public void DefineServices(IServiceCollection services)
    {

    }

    public void DefineEndpoints(WebApplication app)
    {
        var group = app.MapGroup("/basket")
                       .WithTags("Basket");

        group.MapGet("/{userName}", HandleGetBasket)
            .WithName("GetBasket")
            .Produces<ShoppingCart>(StatusCodes.Status200OK);

        // POST /basket
        group.MapPost("/", HandleStoreBasket)
            .WithName("StoreBasket")
            .Produces<ShoppingCart>(StatusCodes.Status201Created);

        group.MapDelete("/{userName}", HandleDeleteBasket)
            .WithName("DeleteBasket")
            .Produces(StatusCodes.Status204NoContent);

        group.MapPost("/checkout", HandleCheckoutBasket)
        .WithName("CheckoutBasket")
        .Produces(StatusCodes.Status202Accepted) 
        .Produces(StatusCodes.Status400BadRequest);
    }

    private static async Task<IResult> HandleCheckoutBasket(CheckoutBasketRequest request, ISender sender)
    {
        var command = request.Adapt<CheckoutBasketCommand>();

        var result = await sender.Send(command);

        if (!result)
        {
            return Results.BadRequest("Basket not found");
        }

        return Results.Accepted();
    }

    private static async Task<IResult> HandleGetBasket(string userName, ISender sender)
    {
        var query = new GetBasketQuery(userName);
        var basket = await sender.Send(query);
        return Results.Ok(basket);
    }

    private static async Task<IResult> HandleStoreBasket(ShoppingCart basket, ISender sender)
    {
        var command = new StoreBasketCommand(basket);
        var updatedBasket = await sender.Send(command);
        return Results.Created($"/basket/{updatedBasket.UserName}", updatedBasket);
    }

    private static async Task<IResult> HandleDeleteBasket(string userName, ISender sender)
    {
        var command = new DeleteBasketCommand(userName);
        await sender.Send(command);
        return Results.NoContent();
    }
}
