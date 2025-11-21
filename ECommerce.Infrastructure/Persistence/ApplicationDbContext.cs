using Microsoft.EntityFrameworkCore;
using ECommerce.Domain.Entities; // Додаємо посилання на наші доменні сутності
using ECommerce.Domain.ValueObjects; // Додаємо посилання на наші об'єкти-значення
using ECommerce.Domain.Common; // Для базового класу Entity, хоча він не використовується напряму тут

namespace ECommerce.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSet'и для наших сутностей. Це те, що EF Core бачитиме як таблиці в базі даних.
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Налаштування для User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id); // Вказуємо, що Id є первинним ключем
                entity.Property(e => e.Email).IsRequired().HasMaxLength(256); // Email обов'язковий і має макс. довжину
                entity.HasIndex(e => e.Email).IsUnique(); // Email має бути унікальним
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.FirstName).HasMaxLength(100);
                entity.Property(e => e.LastName).HasMaxLength(100);

                // Конфігурація об'єкта-значення Address
                entity.OwnsOne(e => e.Address, address =>
                {
                    address.Property(a => a.Street).HasMaxLength(200).HasColumnName("Address_Street");
                    address.Property(a => a.City).HasMaxLength(100).HasColumnName("Address_City");
                    address.Property(a => a.State).HasMaxLength(100).HasColumnName("Address_State");
                    address.Property(a => a.PostalCode).HasMaxLength(20).HasColumnName("Address_PostalCode");
                    address.Property(a => a.Country).HasMaxLength(100).HasColumnName("Address_Country");
                });

                // Додаткове налаштування для EF Core, щоб він використовував приватний конструктор
                entity.HasAnnotation("Relational:ConstructorMapping", "new ECommerce.Domain.Entities.User(string, string, string, string)");
            });

            // Налаштування для Product
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(256);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)"); // Вказуємо тип для десяткових чисел
                entity.Property(e => e.StockQuantity).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.UpdatedAt).IsRequired();

                entity.HasAnnotation("Relational:ConstructorMapping", "new ECommerce.Domain.Entities.Product(string, string, decimal, int)");
            });

            // Налаштування для Order
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.OrderDate).IsRequired();
                entity.Property(e => e.TotalAmount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.UpdatedAt).IsRequired();

                // Конфігурація об'єкта-значення OrderStatus
                entity.Property(e => e.Status)
                      .HasConversion( // Вказуємо EF Core, як конвертувати OrderStatus в/з рядка
                          v => v.Value,
                          v => OrderStatus.FromString(v))
                      .IsRequired()
                      .HasMaxLength(50); // Зберігаємо як рядок в базі даних

                // Зв'язок між Order та User (один користувач може мати багато замовлень)
                entity.HasOne(o => o.User)
                      .WithMany() // У User немає колекції замовлень поки що, можемо додати пізніше
                      .HasForeignKey(o => o.UserId)
                      .OnDelete(DeleteBehavior.Restrict); // Забороняємо видалення користувача, якщо є замовлення

                // Конфігурація для колекції OrderItems
                // EF Core автоматично знайде зв'язок між Order та OrderItem,
                // але нам потрібно явно вказати, що колекція _orderItems є приватною.
                entity.HasMany(o => o.OrderItems)
                      .WithOne(oi => oi.Order)
                      .HasForeignKey(oi => oi.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Вказуємо EF Core, що OrderItems керується приватним полем _orderItems
                var orderItemsNavigation = entity.Metadata.FindNavigation(nameof(Order.OrderItems));
                orderItemsNavigation.SetField("_orderItems");
                orderItemsNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

                entity.HasAnnotation("Relational:ConstructorMapping", "new ECommerce.Domain.Entities.Order(System.Guid)");
            });

            // Налаштування для OrderItem
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Quantity).IsRequired();
                entity.Property(e => e.UnitPrice).HasColumnType("decimal(18,2)");

                // Зв'язок між OrderItem та Order (одна позиція належить одному замовленню)
                entity.HasOne(oi => oi.Order)
                      .WithMany(o => o.OrderItems) // Зв'язок з колекцією в Order
                      .HasForeignKey(oi => oi.OrderId)
                      .OnDelete(DeleteBehavior.Cascade); // При видаленні замовлення видаляються і його позиції

                // Зв'язок між OrderItem та Product (одна позиція відповідає одному продукту)
                entity.HasOne(oi => oi.Product)
                      .WithMany() // У Product немає колекції OrderItems, можемо додати пізніше
                      .HasForeignKey(oi => oi.ProductId)
                      .OnDelete(DeleteBehavior.Restrict); // Забороняємо видалення продукту, якщо він є в замовленні

                entity.HasAnnotation("Relational:ConstructorMapping", "new ECommerce.Domain.Entities.OrderItem(System.Guid, System.Guid, int, decimal)");
            });
        }
    }
}