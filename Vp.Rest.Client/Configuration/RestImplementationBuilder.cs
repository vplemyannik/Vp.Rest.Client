using System;
using System.Collections.Generic;
using System.Net.Http;
using Castle.Core.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Vp.Rest.Client.Authorization;
using Vp.Rest.Client.Authorization.HandlerFactories;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using ILoggerFactory = Microsoft.Extensions.Logging.ILoggerFactory;

namespace Vp.Rest.Client.Configuration
{
    public class RestImplementationBuilder
    {
        private readonly IList<Action<RestMethodOptions>> _actions = new List<Action<RestMethodOptions>>();
        private readonly AuthentificationBuilder _authentificationBuilder = new AuthentificationBuilder();

        public RestImplementationBuilder WithHandler(DelegatingHandler handler)
        {
            _actions.Add(rest => rest.Handlers.Add(handler));
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
        
        public RestImplementationBuilder AddAuthentification(Action<AuthentificationBuilder> authBuilderAction)
        {
            authBuilderAction(_authentificationBuilder);
            return this;
        }

        internal RestImplementation Build(IServiceProvider provider)
        {
            var factories = provider.GetServices<IAuthorizationHandlerFactory>();
            foreach (var handlerFactory in factories)
            {
                var handler = handlerFactory.CreateHandler(_authentificationBuilder.AuthentificationOptions);
                if(handler == null)
                    continue;
                _actions.Add(o => o.Handlers.Add(handler));
            }
            var logger = provider.GetService<ILoggerFactory>();
            if (logger != null)
            {
                _actions.Add(o => o.Handlers.Add(new RequestLoggingHandler(logger.CreateLogger("Request Logger"))));
            }
            
            return Build();
        }
        
        public RestImplementation Build()
        {
            var options = new RestMethodOptions();
            foreach (var action in _actions)
            {
                action(options);
            }
            
            options.Handlers.Add(new TimeOutHandler(options.TimeOut));
            return new RestImplementation(() => new RestMethodInterceptor(Options.Create(options)));
        }
    }
}