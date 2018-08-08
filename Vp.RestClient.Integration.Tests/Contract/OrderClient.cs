using System;
using System.Threading.Tasks;

namespace Vp.RestClient.IntergrationTests.Models
{
    public interface OrderClient
    {
        [Rest(RestMethod.GET, "{orderId}")]
        Task<OrderModel> GetOrder(Guid orderId);
        
        [Rest(RestMethod.POST, "")]
        Task<OrderModel> CreateOrder([Body] OrderModel order);
        
        [Rest(RestMethod.PUT, "{orderId}")]
        Task UpdateOrder([Body] OrderModel order, Guid orderId);
        
        [Rest(RestMethod.DELETE, "{orderId}")]
        Task DeleteOrder(Guid orderId);
    }
}