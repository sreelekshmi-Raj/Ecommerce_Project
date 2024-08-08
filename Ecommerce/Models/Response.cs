namespace Ecommerce.Models
{
    public class Response
    {
        public string ValidationResponse { get; set; }
        public CustomerResponse Customer { get; set; }
        public OrderResponse Order { get; set; }
    }
    public class CustomerResponse
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
    public class OrderResponse
    {
        public int OrderNumber { get; set; }
        public string OrderDate { get; set; }
        public string DeliveryAddress { get; set; }
        public List<OrderItemResponse> OrderItem { get; set; }
        public string DeliveryExpected { get; set; }

    }
    public class OrderItemResponse
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public int PriceEach { get; set; }
    }
}
