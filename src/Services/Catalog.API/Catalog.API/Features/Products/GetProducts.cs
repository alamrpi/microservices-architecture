namespace Catalog.API.Features.Products;

public record ProductDto(Guid Id, string Name, decimal Price);
public record GetProductsResponse(IEnumerable<ProductDto> Products);

public static class GetProductsEndpoint
{
    public static void MapGetProductsEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/", HandleGetProductsAsync)
                    .WithName("GetProducts")
                    .Produces<GetProductsResponse>(StatusCodes.Status200OK)
                    .Produces(StatusCodes.Status404NotFound)
                    .WithSummary("Get All Products");
    }

    private static async Task<IResult> HandleGetProductsAsync()
    {
        var products = new List<ProductDto>
        {
            new ProductDto(Guid.NewGuid(), "Laptop", 1500.00m),
            new ProductDto(Guid.NewGuid(), "Mouse", 70.00m)
        };

        if (products.Count == 0)
        {
            return Results.NotFound();
        }

        var response = new GetProductsResponse(products);
        return Results.Ok(response);
    }
}
