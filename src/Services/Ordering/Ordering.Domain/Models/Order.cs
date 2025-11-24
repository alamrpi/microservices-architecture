using Ordering.Domain.Abstractions;
using Ordering.Domain.Enums;
using Ordering.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.Models
{
    public class Order : Entity<Guid>
    {
        public Guid CustomerId { get; private set; } = default!;
        public string OrderName { get; private set; } = default!;
        public Address ShippingAddress { get; private set; } = default!;
        public Address BillingAddress { get; private set; } = default!;
        public Payment Payment { get; private set; } = default!;
        public OrderStatus Status { get; private set; } = OrderStatus.Draft;

        private readonly List<OrderItem> _orderItems = new();
        public IReadOnlyList<OrderItem> OrderItems => _orderItems.AsReadOnly();

        public decimal TotalPrice
        {
            get => _orderItems.Sum(x => x.Price * x.Quantity);
        }

        public static Order Create(Guid id, Guid customerId, string orderName, Address shippingAddress, Address billingAddress, Payment payment)
        {
            var order = new Order
            {
                Id = id,
                CustomerId = customerId,
                OrderName = orderName,
                ShippingAddress = shippingAddress,
                BillingAddress = billingAddress,
                Payment = payment,
                Status = OrderStatus.Pending
            };

            return order;
        }

        public void Add(Guid productId, int quantity, decimal price)
        {
            var orderItem = new OrderItem(Id, productId, quantity, price);
            _orderItems.Add(orderItem);
        }

        public void Remove(Guid productId)
        {
            var item = _orderItems.FirstOrDefault(x => x.ProductId == productId);
            if (item != null)
            {
                _orderItems.Remove(item);
            }
        }
    }
}
