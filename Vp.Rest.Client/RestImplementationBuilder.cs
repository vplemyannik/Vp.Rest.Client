using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Vp.Rest.Client.Authorization;
using Vp.Rest.Client.Authorization.DelegateConvertor;

namespace Vp.Rest.Client
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

        internal RestImplementation BuildInternal(IServiceProvider provider)
        {
            var factories = provider.GetServices<IAuthorizationHandlerFactory>();
            foreach (var handlerFactory in factories)
            {
                _actions.Add(o => o.Handlers.Add(handlerFactory.CreateHandler(_authentificationBuilder.AuthentificationOptions)));
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

            return new RestImplementation(() => new RestMethodInterceptor(Options.Create(options)));
        }
    }
}