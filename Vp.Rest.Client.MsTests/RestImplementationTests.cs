using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Vp.Rest.Client.MsTests
{
    [TestClass]
    public class RestImplementationTests
    {
        
        public class Order
        {
            public int CustomerName { get; set; }
            public decimal Price { get; set; }
            public List<OrderProduct> Products { get; set; }
            
            public class OrderProduct
            {
                public string Category { get; set; }
                public string Slp { get; set; }
                public int Quantity { get; set; }
            }
        }

        [RestContract("http://localhost:8080/")]
        public interface ITestInterface
        {
            [Rest(RestMethod.POST, "api/orders/")]
            Task GreateOrder([Body] Order order);
        }
        
        [TestMethod]
        public async Task Send()
        {
            var restFactory = new RestImplementationBuilder()
                .AddUrl("http://localhost:8080/")
                .Build();

            var imp = restFactory.Create<ITestInterface>();

            var order = new Fixture().Create<Order>();
            await imp.GreateOrder(order);
        }
    }
}