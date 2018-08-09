using System;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Vp.RestClient.Configuration;
using Vp.RestClient.IntergrationTests.Models;

namespace Vp.RestClient.IntergrationTests
{
    [TestClass]
    public class ApiTests
    {
        private TestServer _server;
        private OrderClient _client;
        private IFixture _fixture;

        [TestInitialize]
        public void Initialize()
        {
            _server =  Initializer.InitializeServer();
            var clientBuilder = new RestImplementationBuilder()
                .WithBaseUrl("http://localhost:54200/api/orders/")
                .WithTimeout(TimeSpan.FromSeconds(30))
                .WithHttpClient(_server.CreateClient())
                .Build();

            _client = clientBuilder.Create<OrderClient>();
            
            _fixture = new Fixture();
        }

        [TestCleanup]
        public void ShutDown()
        {
            _server.Dispose();
        }

        [TestMethod]
        public async Task WorkFlowTests()
        {
            //Arrange CREATE
            var orderModel = _fixture.Create<OrderModel>();
            var createdId = orderModel.Id;
            
            //Act   CREATE
            var retrunModel = await _client.CreateOrder(orderModel);
            
            //Assert CREATE
            Assert.AreNotEqual(createdId, retrunModel.Id);
            
            //Act GET
            var order = await _client.GetOrder(retrunModel.Id);
            
            //Assert GET
            Assert.AreNotEqual(retrunModel, order);

            //Arrange UPDATE
            var oldName = order.CustomerName;
            order.CustomerName = Guid.NewGuid().ToString();
            
            //Act UPDATE
            await _client.UpdateOrder(order);
            
            order = await _client.GetOrder(order.Id);
            
            //Assert UPDATE
            Assert.AreNotEqual(oldName, order.CustomerName);
            
            //Act DELETE
            await _client.DeleteOrder(order.Id);
            
            order = await _client.GetOrder(order.Id);
            
            //Assert DELETE
            Assert.IsNull(order);
        }
        

    }
}