using MediatR;
using Marten;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.API.Features.DeleteBasket;

public record DeleteBasketCommand(string UserName) : IRequest;

public class DeleteBasketCommandHandler : IRequestHandler<DeleteBasketCommand>
{
    private readonly IDocumentSession _martenSession;
    private readonly IDistributedCache _redisCache;

    public DeleteBasketCommandHandler(IDocumentSession martenSession, IDistributedCache redisCache)
    {
        _martenSession = martenSession;
        _redisCache = redisCache;
    }

    public async Task Handle(DeleteBasketCommand request, CancellationToken cancellationToken)
    {
        _martenSession.Delete<Core.Entities.ShoppingCart>(request.UserName);
        await _martenSession.SaveChangesAsync(cancellationToken);

        await _redisCache.RemoveAsync(request.UserName, cancellationToken);
    }
}
