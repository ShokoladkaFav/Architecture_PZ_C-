using ECommerce.Domain.Common;

namespace ECommerce.Domain.Entities
{
    public class Product : Entity // Product успадковує від Entity
    {
        // Приватний конструктор для EF Core
        private Product() : base() { } // <-- ЗМІНА ТУТ: явно викликаємо базовий конструктор без параметрів

        // Основний конструктор
        // Ми більше не передаємо 'id' як параметр, оскільки Entity сам генерує його.
        public Product(string name, string description, decimal price, int stockQuantity, string category)
            : base() // <-- ЗМІНА ТУТ: явно викликаємо базовий конструктор без параметрів
        {
            // Перевірки вхідних даних (можна додати більше валідації)
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Product name cannot be empty.", nameof(name));
            if (price <= 0)
                throw new ArgumentException("Price must be positive.", nameof(price));
            if (stockQuantity < 0)
                throw new ArgumentException("Stock quantity cannot be negative.", nameof(stockQuantity));
            if (string.IsNullOrWhiteSpace(category))
                throw new ArgumentException("Category cannot be empty.", nameof(category));

            Name = name;
            Description = description;
            Price = price;
            StockQuantity = stockQuantity;
            Category = category;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        // Властивості
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }
        public int StockQuantity { get; private set; }
        public string Category { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }


        // Методи для інкапсульованої поведінки
        public void UpdateDetails(string name, string description, decimal price, string category)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Product name cannot be empty.", nameof(name));
            if (price <= 0)
                throw new ArgumentException("Price must be positive.", nameof(price));
            if (string.IsNullOrWhiteSpace(category))
                throw new ArgumentException("Category cannot be empty.", nameof(category));

            Name = name;
            Description = description;
            Price = price;
            Category = category;
            UpdatedAt = DateTime.UtcNow;
        }

        public void IncreaseStock(int quantity)
        {
            if (quantity <= 0) throw new ArgumentException("Quantity must be positive.", nameof(quantity));
            StockQuantity += quantity;
            UpdatedAt = DateTime.UtcNow;
        }

        public void DecreaseStock(int quantity)
        {
            if (quantity <= 0) throw new ArgumentException("Quantity must be positive.", nameof(quantity));
            if (StockQuantity < quantity) throw new InvalidOperationException("Not enough stock.");
            StockQuantity -= quantity;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}