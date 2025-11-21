using ECommerce.Application.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ECommerce.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // ВИПРАВЛЕННЯ ТУТ: Старий синтаксис реєстрації MediatR
            services.AddMediatR(Assembly.GetExecutingAssembly());

            // Реєструємо валідатори
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // Реєструємо Pipeline Behavior
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }
    }
}