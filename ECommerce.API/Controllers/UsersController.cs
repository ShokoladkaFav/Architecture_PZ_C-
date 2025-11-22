using Application.Features.Users.Commands.CreateUser;
using Application.Features.Users.Queries.GetUserById;
using Application.Features.Users.Queries.Login; // <--- ДОДАНО
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST: api/users (Реєстрація)
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserCommand command)
        {
            var userId = await _mediator.Send(command);
            return Ok(userId);
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var query = new GetUserByIdQuery { Id = id };
            var userDto = await _mediator.Send(query);

            if (userDto == null)
            {
                return NotFound($"User with ID {id} not found.");
            }

            return Ok(userDto);
        }

        // ↓↓↓ НОВИЙ МЕТОД ↓↓↓
        // POST: api/users/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserQuery query)
        {
            try
            {
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                // Повертаємо 401 Unauthorized, якщо пароль невірний
                return Unauthorized(ex.Message);
            }
        }
    }
}