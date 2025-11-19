using Catalog.API.Common;
using Mapster;
using MediatR;

namespace Catalog.API.Features.Products;

public class ProductEndpoints : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        var group = app.MapGroup("/products")
                       .WithTags("Products");

        group.MapGet("/", HandleGetProducts)
                    .WithName("GetProducts")
                    .Produces<GetProductsResponse>(StatusCodes.Status200OK);

        // POST /products
        group.MapPost("/", HandleCreateProduct)
            .WithName("CreateProduct")
            .Produces<Guid>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);

        // When we add CreateProduct, it will look like this:
        // group.MapEndpoint(); // This calls GetProducts.MapEndpoint
        // group.MapCreateProductEndpoint(); // This will call CreateProduct.MapEndpoint
    }

    public void DefineServices(IServiceCollection services)
    {

    }


    private static async Task<IResult> HandleGetProducts(ISender sender)
    {
        var query = new GetProductsQuery();
        var response = await sender.Send(query);
        return Results.Ok(response);
    }

    // We inject ISender and the raw HTTP Request DTO
    private static async Task<IResult> HandleCreateProduct(ISender sender, CreateProductRequest request)
    {
        var command = request.Adapt<CreateProductCommand>();

        // Send it to the MediatR pipeline
        // Validation runs automatically!
        var productId = await sender.Send(command);

        return Results.Created($"/products/{productId}", productId);
    }
}
