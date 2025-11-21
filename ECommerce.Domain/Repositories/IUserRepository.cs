using ECommerce.Domain.Entities; // Додайте цей рядок для доступу до User
using ECommerce.Domain.Repositories; // Додайте цей рядок для доступу до IRepository

namespace ECommerce.Domain.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        // Специфічні для користувача методи, наприклад, пошук за Email
        User? GetByEmail(string email);
        Task<User?> GetByEmailAsync(string email);
    }
}