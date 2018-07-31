using System;
using Vp.Rest.Client;

namespace Sample.Google
{
    class Program
    {
        static void Main(string[] args)
        {
            var restImple = new RestImplementationBuilder()
                .WithBaseUrl("fdsfdsds")
                .WithTimeout(TimeSpan.FromSeconds(60))
                .Build();

            var  googleApi = restImple.Create<GoogleApi>();
            var result = googleApi.GetOrder("1");
        }
    }


    public interface GoogleApi
    {
        string GetOrder(string orderId);
    }
}