using MediatR;
using System;

namespace Application.Features.Orders.Events
{
    // Подія: "Замовлення було створено"
    public class OrderCreatedEvent : INotification
    {
        public Guid OrderId { get; }
        public Guid UserId { get; }

        public OrderCreatedEvent(Guid orderId, Guid userId)
        {
            OrderId = orderId;
            UserId = userId;
        }
    }
}