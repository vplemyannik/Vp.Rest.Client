using System;
using System.Collections;
using System.Collections.Generic;

namespace Vp.Rest.Client.Configuration
{
    public class RestClientConfigurationBuilder
    {
        internal IList<(RestImplementationBuilder Builder, Type InterfaceType)> BuilderTypeMap 
            = new List<(RestImplementationBuilder Builder, Type InterfaceType)>();
        
        public RestImplementationBuilder AddClient<TInterface>(string baseUrl)
        {
            var builder = new RestImplementationBuilder();
            builder.WithBaseUrl(baseUrl);
            BuilderTypeMap.Add((builder, typeof(TInterface)));
            return builder;
        }
        
        public RestImplementationBuilder AddClient(string baseUrl, Type interfaceType)
        {
            var builder = new RestImplementationBuilder();
            builder.WithBaseUrl(baseUrl);
            BuilderTypeMap.Add((builder, interfaceType));
            return builder;
        }
    }
}