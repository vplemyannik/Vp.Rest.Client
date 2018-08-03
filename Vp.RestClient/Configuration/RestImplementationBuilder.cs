using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Vp.RestClient.Configuration
{
    public class RestImplementationBuilder
    {
        private readonly IList<Action<RestMethodOptions>> _actions = new List<Action<RestMethodOptions>>();
        private readonly List<DelegatingHandler> _customeHandlers = new List<DelegatingHandler>();

        public RestImplementationBuilder WithHandler(DelegatingHandler handler)
        {
            _customeHandlers.Add(handler);
            return this;
        }
        
        public RestImplementationBuilder WithBaseUrl(string url)
        {
            _actions.Add(r => r.Url = url);
            return this;
        }
        
        public RestImplementationBuilder WithTimeout(TimeSpan timeOut)
        {
            _actions.Add(r => r.TimeOut = timeOut);
            return this;
        }
        
        public RestImplementationBuilder AddLogging(
            Func<HttpRequestMessage, Task> logRequest, 
            Func<HttpResponseMessage, Task> logResponse)
        {
            _actions.Add(o => o.Handlers.Add(new RequestLoggingHandler(logRequest, logResponse)));
            return this;
        }
        
        public RestImplementation Build()
        {
            var options = new RestMethodOptions();
            foreach (var action in _actions)
            {
                action(options);
            }
            
            options.Handlers.Add(new TimeOutHandler(options.TimeOut));
            options.Handlers.AddRange(_customeHandlers);
            return new RestImplementation(() => new RestMethodInterceptor(Options.Create(options)));
        }
    }
}