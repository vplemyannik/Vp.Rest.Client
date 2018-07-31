using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.Extensions.Options;
using Vp.Rest.Client.Authentification;
using Vp.Rest.Client.Authentification.DelegateConvertor;

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
        
        public RestImplementationBuilder AddAuthentification(Action<AuthentificationBuilder> authBuilderAction)
        {
            var authBuilder = new AuthentificationBuilder();
            authBuilderAction(authBuilder);
            var convertors = typeof(IAuthentificationDelegatingConvertor).Assembly
                .GetTypes()
                .Where(t => typeof(IAuthentificationDelegatingConvertor).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            var handlers = new List<DelegatingHandler>();
            foreach (var convertor in convertors)
            {
                var con =  (IAuthentificationDelegatingConvertor) Activator.CreateInstance(convertor);
                handlers.Add(con.Convert(authBuilder.AuthentificationOptionses));
            }
            _actions.Add(o => o.Handlers.AddRange(handlers));
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