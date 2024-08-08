using Ecommerce.Models;

namespace Ecommerce.Repository
{
    public interface IOrderRepository
    {
        Task<Response> GetMostRecentOrderAsync(string email, int customerId);
    }
}
