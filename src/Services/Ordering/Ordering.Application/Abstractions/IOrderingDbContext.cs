
using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Models;
using System.Collections.Generic;

namespace Ordering.Application.Abstractions;

public interface IOrderingDbContext
{
    DbSet<Order> Orders { get; }
    DbSet<OrderItem> OrderItems { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}