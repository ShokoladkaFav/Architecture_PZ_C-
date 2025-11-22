using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using MediatR;
using System;
using System.Linq; // Для Select та ToList
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Application.Features.Orders.Queries.GetOrderById
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetOrderByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<OrderDto> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var repository = _unitOfWork.Repository<Order>();

            // Отримуємо замовлення
            var order = await repository.GetByIdAsync(request.Id);

            if (order == null)
            {
                return null;
            }

            // Отримуємо репозиторій продуктів, щоб підтягнути назви (якщо потрібно)
            // Примітка: У реальному DDD ми б використали .Include(o => o.OrderItems).ThenInclude(i => i.Product)
            // Але поки що зробимо мапінг того, що є.

            var orderDto = new OrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Status = order.Status.ToString(), // Перетворення ValueObject/Enum у рядок
                Items = order.OrderItems.Select(i => new OrderItemDto
                {
                    ProductId = i.ProductId,
                    // ProductName = i.Product?.Name, // Може бути null, якщо Product не завантажився
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
            };

            return orderDto;
        }
    }
}