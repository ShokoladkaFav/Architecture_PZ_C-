using ECommerce.Application.Features.Products.Queries.GetProductById; // Підключаємо DTO з сусідньої папки
using MediatR;
using System.Collections.Generic;

namespace ECommerce.Application.Features.Products.Queries.GetAllProducts
{
    // Запит повертає список (List) об'єктів ProductDto
    public class GetAllProductsQuery : IRequest<List<ProductDto>>
    {
    }
}