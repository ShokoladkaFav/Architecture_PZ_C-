using ECommerce.Domain.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Products.Queries.GetProductById
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto>
    {
        private readonly IProductRepository _productRepository;

        public GetProductByIdQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            // Отримуємо сутність з бази даних
            // Припускаємо, що у вашому базовому репозиторії є метод GetByIdAsync
            var product = await _productRepository.GetByIdAsync(request.Id);

            // Якщо продукт не знайдено — повертаємо null (або можна кидати помилку NotFoundException)
            if (product == null)
            {
                return null;
            }

            // Мапимо (перетворюємо) Сутність у DTO вручну
            // (У великих проектах для цього використовують AutoMapper)
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                Category = product.Category
            };
        }
    }
}