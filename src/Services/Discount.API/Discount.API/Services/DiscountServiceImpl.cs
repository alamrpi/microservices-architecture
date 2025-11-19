using Discount.API.Protos;
using Grpc.Core;

namespace Discount.API.Services;

public class DiscountServiceImpl : DiscountService.DiscountServiceBase
{
    private readonly ILogger<DiscountServiceImpl> _logger;

    public DiscountServiceImpl(ILogger<DiscountServiceImpl> logger)
    {
        _logger = logger;
    }

    public override Task<DiscountModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        _logger.LogInformation($"Getting discount for ProductId: {request.ProductId}");

        var discount = request.ProductId switch
        {
            "product-001" => new DiscountModel { ProductId = "product-001", Description = "Laptop 10% Off", Amount = 150 },
            "product-002" => new DiscountModel { ProductId = "product-002", Description = "Mouse 5% Off", Amount = 5 },
            _ => new DiscountModel { ProductId = request.ProductId, Description = "No Discount", Amount = 0 }
        };

        return Task.FromResult(discount);
    }
}
