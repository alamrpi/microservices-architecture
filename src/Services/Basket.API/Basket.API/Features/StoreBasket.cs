using Basket.API.Core.Entities;
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

    public StoreBasketCommandHandler(IDocumentSession martenSession, IDistributedCache redisCache)
    {
        _martenSession = martenSession;
        _redisCache = redisCache;
    }

    public async Task<ShoppingCart> Handle(StoreBasketCommand request, CancellationToken cancellationToken)
    {
        _martenSession.Store(request.Basket);
        await _martenSession.SaveChangesAsync(cancellationToken);

        await _redisCache.SetStringAsync(
            request.Basket.UserName,
            JsonSerializer.Serialize(request.Basket),
            cancellationToken);

        return request.Basket;
    }
}
