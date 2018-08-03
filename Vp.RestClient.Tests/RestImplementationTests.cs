using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Vp.RestClient.Extensions;
using Vp.RestClient.Configuration;

namespace Vp.RestClient.MsTests
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
                .WithBaseUrl("http://localhost:8080/")
                .WithHandler(new RequestHandlerStub(req =>
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
                .WithBaseUrl("http://localhost:8080/")
                .WithHandler(new RequestHandlerStub(req =>
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
        
        [TestMethod]
        public async Task GetOrder_WithAuth_ShouldBeSuccess()
        {
            //Arrange
            var order = new Fixture()
                .Create<Order>();

            var countInvokation = 0;
            var userName = "Vlad";
            var password = "123";

            var returnedUserName = string.Empty;
            var returnedPassword = string.Empty;
            
            var restFactory = new RestImplementationBuilder()
                .WithBaseUrl("http://localhost:8080/")
                .WithHandler(new BasicAuthHandler(userName, password))
                .WithHandler(new RequestHandlerStub(req =>
                {
                    countInvokation++;
                    string encodedCredentials = req.Headers.Authorization.Parameter;
                    
                    var decodedCredentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));

                    var delimiterIndex = decodedCredentials.IndexOf(':');
                    returnedUserName = decodedCredentials.Substring(0, delimiterIndex);
                    returnedPassword = decodedCredentials.Substring(delimiterIndex + 1);
                    
                    
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
            Assert.AreEqual(userName, returnedUserName);
            Assert.AreEqual(password, returnedPassword);
        }
        
        [TestMethod]
        public async Task GetOrder_WithConfiguration_ShouldBeSuccess()
        {
            //Arrange
            var order = new Fixture()
                .Create<Order>();

            var countInvokation = 0;
            var provider = BuildServiceProvider(services =>
            {
                services.RegisterClients(restBuilder =>
                {
                    restBuilder.AddClient<ITestInterface>("http://localhost:8080/")
                        .WithHandler(new RequestHandlerStub(req =>
                        {
                            countInvokation++;
                            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
                            {
                                Content = new StringContent(JsonConvert.SerializeObject(order), Encoding.UTF8, "application/json")
                            };

                            return responseMessage;
                        }));
                });

                services.AddLogging(b => b.AddDebug());
            });

            var client = provider.GetRequiredService<ITestInterface>();
            
            //Act
            var response = await client.GetOrder(1);
            
            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(1, countInvokation);
        }
        
        
        private IServiceProvider BuildServiceProvider(Action<IServiceCollection> setupServices = null)
        {
            var services = new ServiceCollection();
            services.AddOptions();
            setupServices?.Invoke(services);
            return services.BuildServiceProvider();
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
        
        public class BasicAuthHandler : DelegatingHandler
        {
            private string UserName { get; }
            private string Password { get; }
            
            public BasicAuthHandler(string userName, string password)
            {
                UserName = userName;
                Password = password;
            }
            
            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                var credentials = Convert.ToBase64String(
                    Encoding.UTF8.GetBytes(
                        $"{UserName}:{Password}"
                    )
                );
            
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                return base.SendAsync(request, cancellationToken);
            }
        }
    }
}