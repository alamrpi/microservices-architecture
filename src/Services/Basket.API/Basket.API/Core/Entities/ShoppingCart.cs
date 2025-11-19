using Marten.Schema;

namespace Basket.API.Core.Entities;

public class ShoppingCart
{
    [Identity]
    public string UserName { get; set; } = string.Empty;
    public List<ShoppingCartItem> Items { get; set; } = new();

    public ShoppingCart()
    {
    }

    public ShoppingCart(string userName)
    {
        UserName = userName;
    }
}
