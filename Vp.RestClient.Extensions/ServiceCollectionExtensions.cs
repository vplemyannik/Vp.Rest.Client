using System;
using Microsoft.Extensions.DependencyInjection;

namespace Vp.RestClient.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterClients(this IServiceCollection collection, Action<RestClientConfigurationBuilder> configure)
        {
            var builder = new RestClientConfigurationBuilder();
            configure(builder);

            foreach (var map in builder.BuilderTypeMap)
            {
                collection.AddSingleton(map.InterfaceType, provider =>
                {
                    var factory = map.Builder.Build();
                    return factory.Create(map.InterfaceType);

                });
            }

            return collection;
        }
    }
}