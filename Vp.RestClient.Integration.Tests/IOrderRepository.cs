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
        
        void DeleteOrder(Guid orderId);
    }

    public class OrderRepository : IOrderRepository
    {
        private readonly IDictionary<Guid, Order> _orderStore = new Dictionary<Guid, Order>();
        
        public Order GetOrder(Guid orderId)
        {
            if (!_orderStore.ContainsKey(orderId))
            {
                return null;
            }
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
            _orderStore[order.Id] = order;
        }

        public void DeleteOrder(Guid orderId)
        {
            _orderStore.Remove(orderId);
        }
    }
}