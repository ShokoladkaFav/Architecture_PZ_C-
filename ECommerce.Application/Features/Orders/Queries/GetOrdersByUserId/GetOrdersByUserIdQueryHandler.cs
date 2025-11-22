using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories; // Тут лежить IOrderRepository
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Orders.Queries.GetOrdersByUserId
{
    public class GetOrdersByUserIdQueryHandler : IRequestHandler<GetOrdersByUserIdQuery, List<OrderListDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetOrdersByUserIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<OrderListDto>> Handle(GetOrdersByUserIdQuery request, CancellationToken cancellationToken)
        {
            // Отримуємо репозиторій і приводимо його до типу IOrderRepository
            // Це можливо, бо в UnitOfWork ми тепер створюємо саме OrderRepository
            var orderRepository = _unitOfWork.Repository<Order>() as IOrderRepository;

            if (orderRepository == null)
            {
                return new List<OrderListDto>();
            }

            // Викликаємо спеціальний метод, який робить Include(o => o.OrderItems)
            var orders = await orderRepository.GetOrdersByUserIdAsync(request.UserId);

            // Мапимо результати
            return orders.Select(o => new OrderListDto
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                Status = o.Status.ToString(),
                ItemsCount = o.OrderItems.Count // Це поле буде правильним завдяки Include
            }).ToList();
        }
    }
}