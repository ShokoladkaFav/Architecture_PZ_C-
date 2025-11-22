using MediatR;
using System;
using System.Collections.Generic;

namespace Application.Features.Orders.Commands.CreateOrder
{
    // Головна команда
    public class CreateOrderCommand : IRequest<Guid>
    {
        // UserId видалено, бо ми беремо його з токена
        public List<OrderItemDto> Items { get; set; }
    }

    // Допоміжний клас для передачі списку товарів
    public class OrderItemDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}