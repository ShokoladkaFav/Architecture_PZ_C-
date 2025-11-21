using ECommerce.Application.Features.Products.Commands.CreateProduct;
using ECommerce.Application.Features.Products.Queries.GetProductById;
using ECommerce.Application.Features.Products.Queries.GetAllProducts;
// ↓↓↓ ДОДАЙТЕ ЦІ USING ↓↓↓
// Якщо ці рядки підсвічуються червоним, натисніть Alt+Enter, щоб імпортувати правильний namespace.
// Це залежить від того, чи додали ви "ECommerce." на початку namespace у файлах команд.
using Application.Features.Products.Commands.UpdateProduct;
using Application.Features.Products.Commands.DeleteProduct;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Collections.Generic; // Для List<>

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // 1. GET: Отримання всіх продуктів
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllProductsQuery();
            var products = await _mediator.Send(query);
            return Ok(products);
        }

        // 2. GET: Отримання продукту за ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var query = new GetProductByIdQuery(id); // Якщо у вас конструктор приймає ID
                                                     // АБО: var query = new GetProductByIdQuery { Id = id };

            var product = await _mediator.Send(query);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // 3. POST: Створення продукту
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductCommand command)
        {
            var productId = await _mediator.Send(command);
            return Ok(productId);
        }

        // 4. PUT: Оновлення продукту (НОВЕ)
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductCommand command)
        {
            // Перевіряємо, чи ID в URL співпадає з ID в тілі запиту
            if (id != command.Id)
            {
                return BadRequest("ID in URL does not match ID in body.");
            }

            await _mediator.Send(command);

            // 204 No Content — стандартна відповідь при успішному оновленні
            return NoContent();
        }

        // 5. DELETE: Видалення продукту (НОВЕ)
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteProductCommand { Id = id });

            // 204 No Content — стандартна відповідь при успішному видаленні
            return NoContent();
        }
    }
}