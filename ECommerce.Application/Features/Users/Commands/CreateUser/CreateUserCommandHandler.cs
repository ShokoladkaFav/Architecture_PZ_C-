using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Users.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateUserCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            // TODO: Тут варто було б перевірити, чи існує вже такий Email.

            // Імітація хешування пароля (в реальності тут має бути сервіс)
            var passwordHash = request.Password + "_hashed";

            // Створюємо сутність
            var user = new User(
                request.Email,
                passwordHash,
                request.FirstName,
                request.LastName
            );

            // Зберігаємо
            var repository = _unitOfWork.Repository<User>();
            await repository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return user.Id;
        }
    }
}