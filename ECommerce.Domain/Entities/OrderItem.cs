using ECommerce.Domain.Common;

namespace ECommerce.Domain.Entities
{
    public class OrderItem : Entity
    {
        public Guid OrderId { get; private set; }
        public Guid ProductId { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }

        // Навігаційні властивості
        public Order Order { get; private set; }
        public Product Product { get; private set; }

        public OrderItem(Guid orderId, Guid productId, int quantity, decimal unitPrice)
        {
            OrderId = orderId;
            ProductId = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }
        private OrderItem() { }

        public void UpdateQuantity(int newQuantity)
        {
            if (newQuantity <= 0) throw new ArgumentException("Quantity must be positive.");
            Quantity = newQuantity;
        }

        // Важливо: UnitPrice зазвичай не змінюється після створення OrderItem,
        // оскільки це ціна на момент замовлення. Але якщо є така потреба, можна додати метод.
    }
}