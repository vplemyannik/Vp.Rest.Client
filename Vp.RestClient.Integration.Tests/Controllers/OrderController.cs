using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Vp.RestClient.IntergrationTests.Models;

namespace Vp.RestClient.IntergrationTests.Controllers
{
    [Route("api/orders")]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _repository;

        public OrderController(IOrderRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("{id}")]
        public Task<IActionResult> GetOrder(Guid id)
        {
            var order = _repository.GetOrder(id);

            return Task.FromResult<IActionResult>(Ok(order));
        }
        
        [HttpPost]
        public Task<IActionResult> CreateOrder([FromBody] Order order)
        {
            var id = _repository.CreateOrder(order);
            order.Id = id;
            return Task.FromResult<IActionResult>(Ok(order));
        }
        
        [HttpPut]
        public Task<IActionResult> UpdateOrder([FromBody] Order order)
        {
            _repository.UpdateOrder(order);
            return Task.FromResult<IActionResult>(Ok(order));
        }
        
        [HttpDelete]
        [Route("{id}")]
        public Task<IActionResult> DeleteOrder(Guid id)
        {
            _repository.DeleteOrder(id);
            return Task.FromResult<IActionResult>(Ok());
        }
    }
}