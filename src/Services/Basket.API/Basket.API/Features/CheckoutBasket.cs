// Features/CheckoutBasket/CheckoutBasket.cs
using Basket.API.Core.Dtos;
using Basket.API.Core.Entities;
using FluentValidation;
using Mapster;
using Marten;
using MassTransit;
using MediatR;

namespace Basket.API.Features.CheckoutBasket;

public record CheckoutBasketRequest(BasketCheckoutEvent BasketCheckoutDto);


public record CheckoutBasketCommand(BasketCheckoutEvent BasketCheckoutDto)
    : IRequest<bool>; 

public class CheckoutBasketValidator : AbstractValidator<CheckoutBasketCommand>
{
    public CheckoutBasketValidator()
    {
        RuleFor(x => x.BasketCheckoutDto).NotNull();
        RuleFor(x => x.BasketCheckoutDto.UserName).NotEmpty();
        RuleFor(x => x.BasketCheckoutDto.EmailAddress).NotEmpty().EmailAddress();
    }
}


public class CheckoutBasketHandler : IRequestHandler<CheckoutBasketCommand, bool>
{
    private readonly IDocumentSession _martenSession;
    private readonly IPublishEndpoint _publishEndpoint;

    public CheckoutBasketHandler(IDocumentSession martenSession, IPublishEndpoint publishEndpoint)
    {
        _martenSession = martenSession;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<bool> Handle(CheckoutBasketCommand command, CancellationToken cancellationToken)
    {
        var basket = await _martenSession.LoadAsync<ShoppingCart>(command.BasketCheckoutDto.UserName, cancellationToken);

        if (basket == null)
        {
            return false;
        }


        var eventMessage = command.BasketCheckoutDto;

        eventMessage.TotalPrice = basket.Items.Sum(x => x.Price * x.Quantity);

        await _publishEndpoint.Publish(eventMessage, cancellationToken);

        _martenSession.Delete<ShoppingCart>(command.BasketCheckoutDto.UserName);
        await _martenSession.SaveChangesAsync(cancellationToken);

        return true;
    }
}