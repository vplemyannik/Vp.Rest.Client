using System;
using Microsoft.Extensions.DependencyInjection;

namespace Vp.RestClient.Configuration
{
    public static class ServiceCollectionExtentions
    {
        public static IServiceCollection RegisterClients(
            this IServiceCollection collection
            , Action<RestClientConfigurationBuilder> builderAction)
        {
            var builder = new RestClientConfigurationBuilder();
            builderAction(builder);

//            foreach (var map in builder.BuilderTypeMap)
//            {
//                collection.AddSingleton(map.InterfaceType, provider =>
//                {
//                    var factory = map.Builder.Build(provider);
//                    return factory.Create(map.InterfaceType);
//
//                });
//            }

            return collection;
        }
    }
}