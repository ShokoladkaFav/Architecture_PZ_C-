using System;
using System.Threading;
using System.Threading.Tasks;
using ECommerce.Domain.Common; // <--- Додайте цей using (там де лежить клас Entity)

namespace ECommerce.Domain.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        // Змінюємо 'where T : class' на 'where T : Entity'
        IRepository<T> Repository<T>() where T : Entity;

        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}