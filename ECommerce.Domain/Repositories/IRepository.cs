using ECommerce.Domain.Common;
using System;
using System.Collections.Generic; // <--- Додано для списків
using System.Threading.Tasks;

namespace ECommerce.Domain.Repositories
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        TEntity? GetById(Guid id);
        Task<TEntity?> GetByIdAsync(Guid id);

        // === НОВИЙ МЕТОД ===
        // Повертає список всіх записів
        Task<IReadOnlyList<TEntity>> GetAllAsync();
        // ===================

        void Add(TEntity entity);
        Task AddAsync(TEntity entity);

        void Update(TEntity entity);
        void Remove(TEntity entity);
    }
}