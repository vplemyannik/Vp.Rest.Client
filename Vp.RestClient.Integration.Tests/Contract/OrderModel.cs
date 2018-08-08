using System;
using System.Collections.Generic;

namespace Vp.RestClient.IntergrationTests.Models
{
    public class OrderModel
    {
        public string CustomerName { get; set; }
        public Guid Id { get; set; }
        public int Price { get; set; }
        public List<OrderProduct> Products { get; set; }
            
        public class OrderProduct
        {
            public string Category { get; set; }
            public string Slp { get; set; }
            public int Quantity { get; set; }
        }
    }
}