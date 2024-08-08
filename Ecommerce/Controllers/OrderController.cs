using Ecommerce.Models;
using Ecommerce.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        [HttpPost("get-order")]
        public async Task<IActionResult> GetOrder(Request request)
        {

            var response = await _orderRepository.GetMostRecentOrderAsync(request.user, request.CustomerId);
            return Ok(response);
        }
    }
}
