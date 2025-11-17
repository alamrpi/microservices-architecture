using Catalog.API.Common;

namespace Catalog.API.Features.Products;

public class ProductEndpoints : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        var group = app.MapGroup("/products")
                       .WithTags("Products");

        group.MapGetProductsEndpoint();

        // When we add CreateProduct, it will look like this:
        // group.MapEndpoint(); // This calls GetProducts.MapEndpoint
        // group.MapCreateProductEndpoint(); // This will call CreateProduct.MapEndpoint
    }

    public void DefineServices(IServiceCollection services)
    {

    }
}
