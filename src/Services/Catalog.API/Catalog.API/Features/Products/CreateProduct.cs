using Catalog.API.Core.Entities;
using Catalog.API.Infrastructure.Data;
using FluentValidation;
using Mapster;
using MediatR;

namespace Catalog.API.Features.Products;

public record CreateProductRequest(string Name, string Description, decimal Price, string Category);

public record CreateProductCommand(string Name, string Description, decimal Price, string Category) : IRequest<Guid>;

public class CreateProductValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Price).GreaterThan(0);
        RuleFor(x => x.Category).NotEmpty();
    }
}

// --- Handler ---
public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly CatalogContext _context;

    public CreateProductCommandHandler(CatalogContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        // Map the Command (which is a DTO) to our database Entity
        var product = command.Adapt<Product>();

        // Our validation has already run, so we just add and save
        await _context.Products.AddAsync(product, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return product.Id;
    }
}
