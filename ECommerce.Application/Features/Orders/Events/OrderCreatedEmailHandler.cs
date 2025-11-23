using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Orders.Events
{
    // Цей клас автоматично спрацює, коли хтось зробить Publish(OrderCreatedEvent)
    public class OrderCreatedEmailHandler : INotificationHandler<OrderCreatedEvent>
    {
        private readonly ILogger<OrderCreatedEmailHandler> _logger;

        public OrderCreatedEmailHandler(ILogger<OrderCreatedEmailHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
        {
            // Імітуємо відправку Email (пишемо в консоль)
            _logger.LogInformation("=================================================");
            _logger.LogInformation($"[EMAIL SERVICE]: Sending confirmation email for Order {notification.OrderId} to User {notification.UserId}...");
            _logger.LogInformation("Subject: Your order has been created successfully!");
            _logger.LogInformation("=================================================");

            return Task.CompletedTask;
        }
    }
}