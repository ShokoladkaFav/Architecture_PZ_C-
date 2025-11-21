using ECommerce.Domain.Entities; // Додайте цей рядок для доступу до Order
using ECommerce.Domain.Repositories; // Додайте цей рядок для доступу до IRepository

namespace ECommerce.Domain.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        // Специфічні для замовлення методи
        IEnumerable<Order> GetOrdersByUserId(Guid userId);
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(Guid userId);

        IEnumerable<Order> GetRecentOrders(int count);
        Task<IEnumerable<Order>> GetRecentOrdersAsync(int count);
    }
}