using MassTransit;
using MediatR;
using Ordering.Application.Orders.Commands.CreateOrder;
using Basket.API.Core.Dtos;

namespace Ordering.Application.Orders.EventHandlers;

public class BasketCheckoutConsumer : IConsumer<BasketCheckoutEvent>
{
    private readonly ISender _sender;

    public BasketCheckoutConsumer(ISender sender)
    {
        _sender = sender;
    }

    public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
    {
        var message = context.Message;

        var command = new CreateOrderCommand(
            CustomerId: Guid.NewGuid(),
            UserName: message.UserName,
            TotalPrice: message.TotalPrice,
            FirstName: message.FirstName,
            LastName: message.LastName,
            EmailAddress: message.EmailAddress,
            AddressLine: message.AddressLine,
            Country: message.Country,
            State: message.State,
            ZipCode: message.ZipCode,
            CardName: message.CardName,
            CardNumber: message.CardNumber,
            Expiration: message.Expiration,
            CVV: message.CVV,
            PaymentMethod: message.PaymentMethod
        );
        var orderId = await _sender.Send(command);

        Console.WriteLine($"Order {orderId} created successfully for user {message.UserName}");
    }
}