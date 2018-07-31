using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Vp.Rest.Client.MsTests
{
    [TestClass]
    public class RestImplementationTests
    {
        
        public class Order
        {
            public string CustomerName { get; set; }
            public int Price { get; set; }
            public List<OrderProduct> Products { get; set; }
            
            public class OrderProduct
            {
                public string Category { get; set; }
                public string Slp { get; set; }
                public int Quantity { get; set; }
            }
        }

        public interface ITestInterface
        {
            [Rest(RestMethod.POST, "api/orders/")]
            Task GreateOrder([Body] Order order);

            [Rest(RestMethod.GET, "api/orders/{orderId}")]
            Task<Order> GetOrder(int orderId);
        }
        
        [TestMethod]
        public async Task GreateOrder_WithBody_ShouldBeSuccess()
        {
            //Arrange
            var order = new Fixture()
                .Create<Order>();

            var countInvokation = 0;
            var restFactory = new RestImplementationBuilder()
                .AddUrl("http://localhost:8080/")
                .AddHandler(new RequestHandlerStub(req =>
                {
                    countInvokation++;
                    return new HttpResponseMessage(HttpStatusCode.Created);
                }))
                .Build();

            var imp = restFactory.Create<ITestInterface>();

            //Act
            await imp.GreateOrder(order);
            
            //Assert

            Assert.AreEqual(1, countInvokation);
        }
        
        [TestMethod]
        public async Task GetOrder_ShouldBeSuccess()
        {
            //Arrange
            var order = new Fixture()
                .Create<Order>();

            var countInvokation = 0;
            var restFactory = new RestImplementationBuilder()
                .AddUrl("http://localhost:8080/")
                .AddHandler(new RequestHandlerStub(req =>
                {
                    countInvokation++;
                    var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(order), Encoding.UTF8, "application/json")
                    };

                    return responseMessage;
                }))
                .Build();

            var imp = restFactory.Create<ITestInterface>();

            //Act
            var response = await imp.GetOrder(1);
            
            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(1, countInvokation);
        }

        public class RequestHandlerStub : DelegatingHandler
        {
            private readonly Func<HttpRequestMessage, HttpResponseMessage> _response;
            
            public RequestHandlerStub(Func<HttpRequestMessage, HttpResponseMessage> response)
            {
                _response = response;
            }
            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                return Task.FromResult(_response(request));
            }
        }
    }
}