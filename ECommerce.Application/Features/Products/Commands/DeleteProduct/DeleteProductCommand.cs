using MediatR;
using System;

namespace Application.Features.Products.Commands.DeleteProduct
{
    // Повертаємо Guid (ID видаленого продукту)
    public class DeleteProductCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }
    }
}