using MediatR;
using System;

namespace ECommerce.Application.Features.Products.Queries.GetProductById
{
    // Запит повертає об'єкт ProductDto
    public class GetProductByIdQuery : IRequest<ProductDto>
    {
        public Guid Id { get; set; }

        public GetProductByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}