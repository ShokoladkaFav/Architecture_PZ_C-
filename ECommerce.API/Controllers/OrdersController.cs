using Application.Features.Orders.Commands.CreateOrder;
using Application.Features.Orders.Queries.GetOrderById;
using Application.Features.Orders.Queries.GetOrdersByUserId;
using MediatR;
using Microsoft.AspNetCore.Authorization; // <--- 1. ДОДАНО
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // <--- 2. ДОДАНО: Тепер усі методи цього контролера вимагають Токен
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST: Створення замовлення
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderCommand command)
        {
            var orderId = await _mediator.Send(command);
            return Ok(orderId);
        }

        // GET: Отримання замовлення по ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var query = new GetOrderByIdQuery { Id = id };
            var orderDto = await _mediator.Send(query);

            if (orderDto == null)
            {
                return NotFound($"Order with ID {id} not found.");
            }

            return Ok(orderDto);
        }

        // GET: Отримання історії замовлень користувача
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(Guid userId)
        {
            var query = new GetOrdersByUserIdQuery { UserId = userId };
            var orders = await _mediator.Send(query);

            return Ok(orders);
        }
    }
}