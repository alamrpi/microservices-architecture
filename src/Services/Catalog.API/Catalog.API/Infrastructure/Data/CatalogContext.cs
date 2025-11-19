using Catalog.API.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Infrastructure.Data;

public class CatalogContext : DbContext
{
    public CatalogContext(DbContextOptions<CatalogContext> options) : base(options)
    {
    }
    public DbSet<Product> Products { get; set; }
}
