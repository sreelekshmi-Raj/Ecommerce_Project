using Dapper;
using Ecommerce.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Ecommerce.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly string _connectionString;

        public OrderRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Response> GetMostRecentOrderAsync(string email, int customerId)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var customerQuery = "SELECT * FROM CUSTOMERS WHERE EMAIL = @Email AND CUSTOMERID = @CustomerId";
                var customer = await db.QuerySingleOrDefaultAsync<Customer>(customerQuery, new { Email = email, CustomerId = customerId });

                if (customer == null)
                {
                    return new Response
                    {
                        ValidationResponse= "Invalid customer details.",
                        Customer = null,                       
                    };
                }

                var orderQuery = @"
                SELECT TOP 1 * FROM ORDERS
                WHERE CUSTOMERID = @CustomerId
                ORDER BY ORDERDATE DESC";

                var order = await db.QuerySingleOrDefaultAsync<Order>(orderQuery, new { CustomerId = customerId });

                if (order == null)
                {
                    return new Response
                    {
                        ValidationResponse="SUCESS",
                        Customer = new CustomerResponse
                        {
                            FirstName=customer.FirstName,
                            LastName=customer.LastName,
                        },
                        Order = null
                    };

                }

                var orderItemsQuery = @"
                SELECT p.PRODUCTNAME AS Product, o.QUANTITY AS Quantity, o.PRICE AS PriceEach
                FROM ORDERITEMS o
                JOIN PRODUCTS p ON o.PRODUCTID = p.PRODUCTID
                WHERE o.ORDERID = @OrderId";

                var orderItems = await db.QueryAsync<OrderItemResponse>(orderItemsQuery, new { OrderId = order.OrderId });

                // Handle gift items
                if (order.ContainsGift)
                {
                    foreach (var item in orderItems)
                    {
                        item.ProductName = "Gift";
                    }
                }

                return new Response
                {
                    ValidationResponse="SUCESS",
                    Customer = new CustomerResponse { 
                        FirstName=customer.FirstName,
                        LastName=customer.LastName
                    },
                    Order = new OrderResponse
                    {
                        OrderNumber = order.OrderId,
                        OrderDate = order.OrderDate.ToString("dd-MMM-yyyy"),
                        DeliveryAddress = $"{customer.HouseNo} {customer.Street}, {customer.Town}, {customer.PostCode}",
                        OrderItem = orderItems.ToList(),
                        DeliveryExpected = order.DeliveryExpected.ToString("dd-MMM-yyyy")
                    }
                };
            }

        }
    }
}
            
