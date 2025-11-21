using ECommerce.Application.Common.Exceptions; // Тут лежить наш ValidationException
using System.Net;
using System.Text.Json;

namespace ECommerce.API.Middleware
{
    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                // Пропускаємо запит далі по конвеєру
                await _next(context);
            }
            catch (Exception ex)
            {
                // Якщо сталася помилка — ловимо її тут
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError; // За замовчуванням 500
            var result = string.Empty;

            switch (exception)
            {
                // Якщо помилка типу ValidationException (наша)
                case ValidationException validationException:
                    code = HttpStatusCode.BadRequest; // Ставимо код 400
                    // Серіалізуємо список помилок у JSON
                    result = JsonSerializer.Serialize(new { errors = validationException.Errors });
                    break;

                // Тут можна додати інші типи помилок (наприклад, NotFound -> 404)

                // Всі інші, невідомі помилки
                default:
                    code = HttpStatusCode.InternalServerError;
                    result = JsonSerializer.Serialize(new { error = exception.Message });
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            if (string.IsNullOrEmpty(result))
            {
                result = JsonSerializer.Serialize(new { error = exception.Message });
            }

            return context.Response.WriteAsync(result);
        }
    }
}