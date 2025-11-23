using Application.Features.Orders.Events; // <--- Додано посилання на подію
using Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMediator _mediator; // <--- Додано поле для публікації подій

        // Оновлений конструктор
        public CreateOrderCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IMediator mediator) // <--- Інжектимо IMediator
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _mediator = mediator;
        }

        public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            // 1. Отримуємо ID користувача
            var userId = _currentUserService.UserId;

            if (userId == Guid.Empty)
            {
                throw new UnauthorizedAccessException("User ID not found in token.");
            }

            var userRepo = _unitOfWork.Repository<User>();
            var user = await userRepo.GetByIdAsync(userId);

            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }

            // 2. Створення замовлення
            var order = new Order(userId);
            var productRepo = _unitOfWork.Repository<Product>();

            // 3. Обробка товарів
            foreach (var itemDto in request.Items)
            {
                var product = await productRepo.GetByIdAsync(itemDto.ProductId);

                if (product == null)
                {
                    throw new KeyNotFoundException($"Product with ID {itemDto.ProductId} not found.");
                }

                try
                {
                    product.DecreaseStock(itemDto.Quantity);
                }
                catch (InvalidOperationException ex)
                {
                    throw new InvalidOperationException($"Product '{product.Name}': {ex.Message}");
                }

                order.AddOrderItem(product.Id, itemDto.Quantity, product.Price);
            }

            // 4. Збереження
            var orderRepo = _unitOfWork.Repository<Order>();
            await orderRepo.AddAsync(order);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // ↓↓↓ 5. ПУБЛІКАЦІЯ ПОДІЇ (EVENT DRIVEN DESIGN) ↓↓↓
            // Це запустить OrderCreatedEmailHandler у фоновому режимі
            await _mediator.Publish(new OrderCreatedEvent(order.Id, userId), cancellationToken);
            // =================================================

            return order.Id;
        }
    }
}