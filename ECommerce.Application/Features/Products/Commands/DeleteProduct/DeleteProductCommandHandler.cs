using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Products.Commands.DeleteProduct
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProductCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            // 1. Отримуємо репозиторій
            var repository = _unitOfWork.Repository<Product>();

            // 2. Знаходимо продукт
            var product = await repository.GetByIdAsync(request.Id);

            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID {request.Id} not found.");
            }

            // 3. Видаляємо (використовуємо метод Remove, який ми додали в Generic Repository)
            repository.Remove(product);

            // 4. Зберігаємо зміни
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return product.Id;
        }
    }
}