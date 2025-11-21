using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using MediatR;

namespace ECommerce.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        // Впроваджуємо залежності через конструктор
        public CreateProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            // 1. Створюємо нову сутність Product.
            // Ми використовуємо конструктор сутності для забезпечення валідності доменної моделі (DDD).
            // Передбачається, що у класі Product (ECommerce.Domain) є відповідний публічний конструктор.
            var product = new Product(
                request.Name,
                request.Description,
                request.Price,
                request.StockQuantity,
                request.Category
            );

            // 2. Додаємо сутність до репозиторію (це лише додає її до контексту EF в пам'яті/Tracking)
            await _productRepository.AddAsync(product);

            // 3. Зберігаємо зміни в базі даних за допомогою UnitOfWork
            // Саме тут генерується SQL INSERT
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // 4. Повертаємо ID новоствореного продукту
            return product.Id;
        }
    }
}