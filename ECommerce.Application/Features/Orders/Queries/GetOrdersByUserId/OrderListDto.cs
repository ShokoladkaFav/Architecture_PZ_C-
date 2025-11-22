using System;

namespace Application.Features.Orders.Queries.GetOrdersByUserId
{
    public class OrderListDto
    {
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public int ItemsCount { get; set; } // Кількість товарів у чеку
    }
}