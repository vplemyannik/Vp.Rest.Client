using System;
using System.Collections.Generic;
using Vp.RestClient.IntergrationTests.Models;

namespace Vp.RestClient.IntergrationTests
{
    public interface IOrderRepository
    {
        Order GetOrder(Guid orderId);
        
        Guid CreateOrder(Order order);
        
        void UpdateOrder(Order order);
        
        void DeleteOrder(Order order);
    }

    public class OrderRepository : IOrderRepository
    {
        private readonly IDictionary<Guid, Order> _orderStore = new Dictionary<Guid, Order>();
        
        public Order GetOrder(Guid orderId)
        {
            return _orderStore[orderId];
        }

        public Guid CreateOrder(Order order)
        {
            var id = Guid.NewGuid();
            _orderStore.Add(id, order);
            return id;
        }

        public void UpdateOrder(Order order)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteOrder(Order order)
        {
            throw new System.NotImplementedException();
        }
    }
}