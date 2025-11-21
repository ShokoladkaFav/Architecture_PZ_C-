using ECommerce.Domain.Entities; // Доступ до Order
using ECommerce.Domain.Repositories; // Доступ до IOrderRepository
using ECommerce.Infrastructure.Persistence; // Доступ до ApplicationDbContext
using Microsoft.EntityFrameworkCore; // Для .Where, .OrderByDescending та .ToListAsync

namespace ECommerce.Infrastructure.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        // Реалізація специфічних методів IOrderRepository
        public IEnumerable<Order> GetOrdersByUserId(Guid userId)
        {
            // Отримання замовлень користувача. Включаємо OrderItems, якщо потрібно.
            return _dbSet.Where(o => o.UserId == userId)
                         .Include(o => o.OrderItems) // Завантажуємо пов'язані OrderItems
                         .ToList();
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(Guid userId)
        {
            return await _dbSet.Where(o => o.UserId == userId)
                               .Include(o => o.OrderItems) // Асинхронно завантажуємо пов'язані OrderItems
                               .ToListAsync();
        }

        public IEnumerable<Order> GetRecentOrders(int count)
        {
            // Отримання останніх замовлень, сортуючи за датою створення
            return _dbSet.OrderByDescending(o => o.OrderDate)
                         .Include(o => o.OrderItems)
                         .Take(count)
                         .ToList();
        }

        public async Task<IEnumerable<Order>> GetRecentOrdersAsync(int count)
        {
            return await _dbSet.OrderByDescending(o => o.OrderDate)
                               .Include(o => o.OrderItems)
                               .Take(count)
                               .ToListAsync();
        }
    }
}