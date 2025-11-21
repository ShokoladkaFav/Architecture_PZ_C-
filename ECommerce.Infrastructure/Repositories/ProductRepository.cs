using ECommerce.Domain.Entities; // Доступ до Product
using ECommerce.Domain.Repositories; // Доступ до IProductRepository
using ECommerce.Infrastructure.Persistence; // Доступ до ApplicationDbContext
using Microsoft.EntityFrameworkCore; // Для .Where та .ToListAsync

namespace ECommerce.Infrastructure.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        // Реалізація специфічних методів IProductRepository
        public IEnumerable<Product> GetProductsByCategory(string categoryName)
        {
            // Отримання продуктів, які містять в назві категорії задану підстроку
            // Тут може бути більш складна логіка, наприклад, enum або окрема таблиця категорій
            return _dbSet.Where(p => p.Category.Contains(categoryName)).ToList();
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(string categoryName)
        {
            return await _dbSet.Where(p => p.Category.Contains(categoryName)).ToListAsync();
        }
    }
}