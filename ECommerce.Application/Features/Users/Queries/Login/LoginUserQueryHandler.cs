using Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Users.Queries.Login
{
    public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, LoginUserDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;

        public LoginUserQueryHandler(IUnitOfWork unitOfWork, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
        }

        public async Task<LoginUserDto> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            // Отримуємо спеціальний UserRepository через UnitOfWork
            var userRepository = _unitOfWork.Repository<User>() as IUserRepository;

            if (userRepository == null)
            {
                throw new InvalidOperationException("User repository configuration error.");
            }

            // Шукаємо користувача
            var user = await userRepository.GetByEmailAsync(request.Email);

            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            // Перевіряємо пароль (імітація хешування, як при реєстрації)
            var inputPasswordHashed = request.Password + "_hashed";

            if (user.PasswordHash != inputPasswordHashed)
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            // Генеруємо токен
            var token = _tokenService.GenerateToken(user.Id, user.Email, user.FirstName, user.LastName);

            return new LoginUserDto
            {
                Email = user.Email,
                Token = token
            };
        }
    }
}