using ECommerce.Domain.Repositories;
using ECommerce.Domain.Common;
using ECommerce.Domain.Entities; // <--- Додано для доступу до User та Order
using ECommerce.Infrastructure.Persistence;
using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        private Hashtable _repositories;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public IRepository<T> Repository<T>() where T : Entity
        {
            if (_repositories == null)
                _repositories = new Hashtable();

            var type = typeof(T).Name;

            if (!_repositories.ContainsKey(type))
            {
                object repositoryInstance;

                // === ЛОГІКА ВИБОРУ СПЕЦІАЛЬНИХ РЕПОЗИТОРІЇВ ===

                if (typeof(T) == typeof(Order))
                {
                    // Використовуємо ваш OrderRepository
                    repositoryInstance = new OrderRepository(_dbContext);
                }
                else if (typeof(T) == typeof(User))
                {
                    // Використовуємо ваш UserRepository
                    repositoryInstance = new UserRepository(_dbContext);
                }
                else
                {
                    // Для всіх інших (наприклад, Product) - стандартний Generic Repository
                    var repositoryType = typeof(Repository<>);
                    repositoryInstance = Activator.CreateInstance(
                        repositoryType.MakeGenericType(typeof(T)),
                        _dbContext
                    );
                }
                // ================================================

                _repositories.Add(type, repositoryInstance);
            }

            return (IRepository<T>)_repositories[type];
        }
    }
}