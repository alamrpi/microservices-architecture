using Basket.API.Core.Entities;
using MediatR;
using Marten;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.API.Features.GetBasket;

public record GetBasketQuery(string UserName) : IRequest<ShoppingCart>;

public class GetBasketQueryHandler : IRequestHandler<GetBasketQuery, ShoppingCart>
{
    private readonly IDocumentSession _martenSession;
    private readonly IDistributedCache _redisCache;

    public GetBasketQueryHandler(IDocumentSession martenSession, IDistributedCache redisCache)
    {
        _martenSession = martenSession;
        _redisCache = redisCache;
    }

    public async Task<ShoppingCart> Handle(GetBasketQuery request, CancellationToken cancellationToken)
    {
        var cachedBasket = await _redisCache.GetStringAsync(request.UserName, cancellationToken);

        if (!string.IsNullOrEmpty(cachedBasket))
        {
            return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket)!;
        }

        var basket = await _martenSession.LoadAsync<ShoppingCart>(request.UserName, cancellationToken);

        if (basket is null)
        {
            basket = new ShoppingCart(request.UserName);
        }

        await _redisCache.SetStringAsync(
            request.UserName,
            JsonSerializer.Serialize(basket),
            cancellationToken);

        return basket;
    }
}
