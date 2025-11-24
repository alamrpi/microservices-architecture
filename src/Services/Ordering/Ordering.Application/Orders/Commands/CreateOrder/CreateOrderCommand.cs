using Mapster;
using MediatR;
using Ordering.Application.Abstractions;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;

namespace Ordering.Application.Orders.Commands.CreateOrder;

public record CreateOrderCommand(
    Guid CustomerId,
    string UserName,
    decimal TotalPrice,
    string FirstName, string LastName, string EmailAddress, string AddressLine, string Country, string State, string ZipCode,
    string CardName, string CardNumber, string Expiration, string CVV, int PaymentMethod
) : IRequest<Guid>;

// 2. Handler
public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
{
    private readonly IOrderingDbContext _context;

    public CreateOrderCommandHandler(IOrderingDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var shippingAddress = new Address(command.FirstName, command.LastName, command.EmailAddress, command.AddressLine, command.Country, command.State, command.ZipCode);
        var billingAddress = new Address(command.FirstName, command.LastName, command.EmailAddress, command.AddressLine, command.Country, command.State, command.ZipCode);

        var payment = new Payment(command.CardName, command.CardNumber, command.Expiration, command.CVV, command.PaymentMethod);

        var orderId = Guid.NewGuid();

        var order = Order.Create(
            id: orderId,
            customerId: command.CustomerId,
            orderName: $"{command.UserName}'s Order",
            shippingAddress: shippingAddress,
            billingAddress: billingAddress,
            payment: payment
        );

        _context.Orders.Add(order);
        await _context.SaveChangesAsync(cancellationToken);

        return order.Id;
    }
}