using System;
using System.Collections.Generic;
using System.Net.Http;
using Castle.DynamicProxy;
using Microsoft.Extensions.Options;

namespace Vp.Rest.Client
{
    public class RestImplementationBuilder
    {
        private readonly IList<Action<RestMethodOptions>> _actions = new List<Action<RestMethodOptions>>();

        public RestImplementationBuilder AddHandler(DelegatingHandler handler)
        {
            _actions.Add(rest => rest.Handlers.Add(handler));
            return this;
        }
        
        public RestImplementationBuilder AddUrl(string url)
        {
            _actions.Add(r => r.Url = url);
            return this;
        }
        
        public RestImplementationBuilder AddTimeout(TimeSpan timeOut)
        {
            _actions.Add(r => r.TimeOut = timeOut);
            return this;
        }
        
        public RestImplementation Build()
        {
            var options = new RestMethodOptions();
            foreach (var action in _actions)
            {
                action(options);
            }

            return new RestImplementation(() => new RestMethodInterceptor(Options.Create(options)));
        }
    }
}