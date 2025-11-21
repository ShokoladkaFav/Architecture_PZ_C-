namespace ECommerce.Domain.Common
{
    public abstract class Entity
    {
        public Guid Id { get; protected set; } // protected set дозволяє встановлювати Id тільки всередині класу або похідних класів

        protected Entity()
        {
            Id = Guid.NewGuid(); // Автоматично генеруємо Id при створенні сутності
        }

        // Можна додати базові методи для порівняння сутностей за Id, якщо потрібно
    }
}