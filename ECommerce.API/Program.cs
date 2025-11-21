using Microsoft.EntityFrameworkCore;
using ECommerce.Infrastructure.Persistence;
using ECommerce.Domain.Repositories;
using ECommerce.Infrastructure.Repositories;
using ECommerce.Application;
using ECommerce.API.Middleware; // <--- 1. ДОДАНО: Щоб бачити папку Middleware

namespace ECommerce.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Реєстрація ApplicationDbContext
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Реєстрація Unit of Work
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Реєстрація специфічних репозиторіїв
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();

            // Реєстрація Application Layer (MediatR, Validators, Behaviors)
            builder.Services.AddApplication();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            // ====== 2. ДОДАНО: Підключення глобального обробника помилок ======
            // Це перехоплює помилки валідації і перетворює 500 на 400
            app.UseMiddleware<CustomExceptionHandlerMiddleware>();
            // ==================================================================

            app.MapControllers();

            app.Run();
        }
    }
}