using ECommerce.Domain.Entities; // Додайте цей рядок для доступу до Product
using ECommerce.Domain.Repositories; // Додайте цей рядок для доступу до IRepository

namespace ECommerce.Domain.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        // Специфічні для продукту методи, якщо вони потрібні
        // Наприклад, GetProductsByCategory, GetAvailableProducts
        IEnumerable<Product> GetProductsByCategory(string categoryName);
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(string categoryName);
    }
}