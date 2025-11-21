using ECommerce.Domain.Common;
using ECommerce.Domain.ValueObjects; // Додаємо посилання

namespace ECommerce.Domain.Entities
{
    public class User : Entity
    {
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        // Об'єкт-значення Address
        public Address Address { get; private set; } // EF Core може зберігати Value Objects, якщо їх відобразити

        public User(string email, string passwordHash, string firstName, string lastName)
        {
            Email = email;
            PasswordHash = passwordHash;
            FirstName = firstName;
            LastName = lastName;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
        private User() { }

        public void UpdateName(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateEmail(string email)
        {
            Email = email;
            UpdatedAt = DateTime.UtcNow;
        }

        // Метод для встановлення Address
        public void SetAddress(Address address)
        {
            Address = address;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}