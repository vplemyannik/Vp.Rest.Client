using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace Vp.RestClient.IntergrationTests
{
    public static class Initializer
    {
        public static TestServer InitializeServer()
        {
            var webBuilder = new WebHostBuilder()
                .UseStartup<Startup>();

            var server = new TestServer(webBuilder)
            {
                BaseAddress = new Uri("http://localhost:54200", UriKind.Absolute)
            };

            return server;
        }
    }
}