using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Vp.Rest.Client
{
    public class RestImplementationBuilder
    {
        private readonly IList<Action<RestImplementation>> _actions = new List<Action<RestImplementation>>();

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
            var implementation = new RestImplementation();
            foreach (var action in _actions)
            {
                action(implementation);
            }

            return implementation;
        }
    }
}