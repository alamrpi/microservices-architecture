using Basket.API.Core.Entities;
using Basket.API.Protos;
using MediatR;
using Marten;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.API.Features.StoreBasket;

public record StoreBasketCommand(ShoppingCart Basket) : IRequest<ShoppingCart>;

public class StoreBasketCommandHandler : IRequestHandler<StoreBasketCommand, ShoppingCart>
{
    private readonly IDocumentSession _martenSession;
    private readonly IDistributedCache _redisCache;
    private readonly DiscountService.DiscountServiceClient _discountClient;

    public StoreBasketCommandHandler(
        IDocumentSession martenSession,
        IDistributedCache redisCache,
        DiscountService.DiscountServiceClient discountClient)
    {
        _martenSession = martenSession;
        _redisCache = redisCache;
        _discountClient = discountClient;
    }

    public async Task<ShoppingCart> Handle(StoreBasketCommand request, CancellationToken cancellationToken)
    {
        foreach (var item in request.Basket.Items)
        {
            var coupon = await _discountClient.GetDiscountAsync(
                new GetDiscountRequest { ProductId = item.ProductId.ToString() },
                cancellationToken: cancellationToken);
            item.Price -= (decimal)coupon.Amount;
        }

        _martenSession.Store(request.Basket);
        await _martenSession.SaveChangesAsync(cancellationToken);

        await _redisCache.SetStringAsync(
            request.Basket.UserName,
            JsonSerializer.Serialize(request.Basket),
            cancellationToken);

        return request.Basket;
    }
}
