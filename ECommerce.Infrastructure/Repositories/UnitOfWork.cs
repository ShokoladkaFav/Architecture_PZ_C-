using ECommerce.Domain.Repositories;
using ECommerce.Domain.Common; // Тут лежить клас Entity
using ECommerce.Infrastructure.Persistence;
using System;
using System.Collections; // Потрібно для Hashtable
using System.Threading;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        private Hashtable _repositories; // Колекція для зберігання створених репозиторіїв

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

        // ↓↓↓ ОСЬ ЦЕЙ МЕТОД БУВ ВІДСУТНІЙ ↓↓↓
        public IRepository<T> Repository<T>() where T : Entity
        {
            if (_repositories == null)
                _repositories = new Hashtable();

            var type = typeof(T).Name;

            // Перевіряємо, чи вже створено репозиторій для цього типу, щоб не створювати дублікати
            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(Repository<>);

                // Створюємо екземпляр Repository<T>, передаючи контекст бази даних
                var repositoryInstance = Activator.CreateInstance(
                    repositoryType.MakeGenericType(typeof(T)),
                    _dbContext
                );

                _repositories.Add(type, repositoryInstance);
            }

            return (IRepository<T>)_repositories[type];
        }
    }
}