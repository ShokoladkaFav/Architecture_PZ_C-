using ECommerce.Domain.Common;
using System.Collections.Generic;
using System.Linq;

namespace ECommerce.Domain.ValueObjects
{
    public class OrderStatus : ValueObject
    {
        public string Value { get; private set; }

        // Приватний конструктор для Entity Framework Core
        private OrderStatus() { }

        private OrderStatus(string value)
        {
            Value = value;
        }

        // Статичні властивості для доступних статусів
        public static OrderStatus New => new OrderStatus("New");
        public static OrderStatus Processing => new OrderStatus("Processing");
        public static OrderStatus Shipped => new OrderStatus("Shipped");
        public static OrderStatus Delivered => new OrderStatus("Delivered");
        public static OrderStatus Cancelled => new OrderStatus("Cancelled");

        // Метод для створення OrderStatus з рядка
        public static OrderStatus FromString(string status)
        {
            return status switch
            {
                "New" => New,
                "Processing" => Processing,
                "Shipped" => Shipped,
                "Delivered" => Delivered,
                "Cancelled" => Cancelled,
                _ => throw new ArgumentException($"Invalid order status: {status}")
            };
        }

        // Додаткові методи для перевірки стану
        public bool IsNew() => this == New;
        public bool IsProcessing() => this == Processing;
        public bool IsShipped() => this == Shipped;
        public bool IsDelivered() => this == Delivered;
        public bool IsCancelled() => this == Cancelled;

        // Реалізація абстрактного методу для порівняння ValueObject
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}