using ECommerce.Domain.Common;
using ECommerce.Domain.ValueObjects; // Додаємо посилання
using System;
using System.Collections.Generic;
using System.Linq;

namespace ECommerce.Domain.Entities
{
    public class Order : Entity
    {
        public Guid UserId { get; private set; }
        public DateTime OrderDate { get; private set; }
        public decimal TotalAmount { get; private set; }
        public OrderStatus Status { get; private set; } // Тепер це Value Object
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; set; } // Змінено на public set для простоти UpdatedAt в агрегаті

        private readonly List<OrderItem> _orderItems = new List<OrderItem>();
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

        public User User { get; private set; }

        public Order(Guid userId)
        {
            UserId = userId;
            OrderDate = DateTime.UtcNow;
            Status = OrderStatus.New; // Використовуємо статичну властивість Value Object
            TotalAmount = 0;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
        private Order() { }

        public void AddOrderItem(Guid productId, int quantity, decimal unitPrice)
        {
            if (Status.IsCancelled() || Status.IsDelivered())
                throw new InvalidOperationException("Cannot add items to a cancelled or delivered order.");
            // ... (решта логіки така ж)
            var existingItem = _orderItems.FirstOrDefault(item => item.ProductId == productId);
            if (existingItem != null)
            {
                existingItem.UpdateQuantity(existingItem.Quantity + quantity);
            }
            else
            {
                _orderItems.Add(new OrderItem(Id, productId, quantity, unitPrice));
            }
            RecalculateTotalAmount();
            UpdatedAt = DateTime.UtcNow;
        }

        public void RemoveOrderItem(Guid orderItemId)
        {
            if (Status.IsCancelled() || Status.IsDelivered())
                throw new InvalidOperationException("Cannot remove items from a cancelled or delivered order.");
            // ... (решта логіки така ж)
            var itemToRemove = _orderItems.FirstOrDefault(item => item.Id == orderItemId);
            if (itemToRemove == null) throw new InvalidOperationException("Order item not found in order.");

            _orderItems.Remove(itemToRemove);
            RecalculateTotalAmount();
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateOrderItemQuantity(Guid orderItemId, int newQuantity)
        {
            if (Status.IsCancelled() || Status.IsDelivered())
                throw new InvalidOperationException("Cannot update items in a cancelled or delivered order.");
            // ... (решта логіки така ж)
            var itemToUpdate = _orderItems.FirstOrDefault(item => item.Id == orderItemId);
            if (itemToUpdate == null) throw new InvalidOperationException("Order item not found in order.");
            itemToUpdate.UpdateQuantity(newQuantity);
            RecalculateTotalAmount();
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetStatus(OrderStatus newStatus) // Тепер приймаємо OrderStatus
        {
            // Додаємо логіку переходу статусів, якщо потрібно
            if (Status.IsDelivered() && !newStatus.IsDelivered() && !newStatus.IsCancelled())
                throw new InvalidOperationException("Cannot change status from Delivered to non-final status.");
            // Можна додати інші правила переходу
            Status = newStatus;
            UpdatedAt = DateTime.UtcNow;
        }

        private void RecalculateTotalAmount()
        {
            TotalAmount = _orderItems.Sum(item => item.Quantity * item.UnitPrice);
        }
    }
}