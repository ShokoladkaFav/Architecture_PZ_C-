using ECommerce.Domain.Repositories;
using ECommerce.Domain.Entities;
using MediatR;
using System; // Для Guid та KeyNotFoundException
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            // 1. Отримуємо репозиторій
            var repository = _unitOfWork.Repository<Product>();

            // 2. Шукаємо продукт за ID
            var product = await repository.GetByIdAsync(request.Id);

            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID {request.Id} not found.");
            }

            // 3. Оновлюємо дані через метод сутності (DDD підхід)
            // Використовуємо існуючий метод UpdateDetails
            product.UpdateDetails(
                request.Name,
                request.Description,
                request.Price,
                request.Category
            );

            // 4. Зберігаємо зміни
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}