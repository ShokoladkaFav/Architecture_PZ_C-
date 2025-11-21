using ECommerce.Domain.Entities; // Доступ до User
using ECommerce.Domain.Repositories; // Доступ до IUserRepository
using ECommerce.Infrastructure.Persistence; // Доступ до ApplicationDbContext
using Microsoft.EntityFrameworkCore; // Для .FirstOrDefaultAsync

namespace ECommerce.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        // Конструктор викликає конструктор базового класу (Repository)
        public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        // Реалізація специфічних методів IUserRepository
        public User? GetByEmail(string email)
        {
            return _dbSet.FirstOrDefault(u => u.Email == email);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}