using Catalog.API.Infrastructure.Data;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Features.Products;

public record ProductDto(Guid Id, string Name, decimal Price);
public record GetProductsResponse(IEnumerable<ProductDto> Products);

public record GetProductsQuery : IRequest<GetProductsResponse>;

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, GetProductsResponse>
{
    private readonly CatalogContext _context;

    public GetProductsQueryHandler(CatalogContext context)
    // Inject the DbContext
    {
        _context = context;
    }

    public async Task<GetProductsResponse> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        var products = await _context.Products
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        // Use Mapster to map List<Product> to List<ProductDto>
        var productDtos = products.Adapt<IEnumerable<ProductDto>>();

        return new GetProductsResponse(productDtos);
    }
}
