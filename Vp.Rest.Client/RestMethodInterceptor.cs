using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Castle.DynamicProxy;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Options;

namespace Vp.Rest.Client
{
    public class RestMethodInterceptor : IRestMethodInterceptor
    {
        private readonly IOptions<RestMethodOptions> _options;

        public RestMethodInterceptor(IOptions<RestMethodOptions> options)
        {
            _options = options;
        }

        public void Intercept(IInvocation invocation)
        {
            var method = invocation.GetConcreteMethod();
            if (method.ReturnType == typeof(string))
            {
                invocation.ReturnValue = method.Name;
                return;
            }
        }

        private HttpClient CreateHttpClient(IInvocation invocation, HttpMessageHandler handler)
        {
            return new HttpClient(CreateHttpMessageHandler(invocation));
        }
        
        private HttpMessageHandler CreateHttpMessageHandler(IInvocation invocation)
        {
            
            var primatyHandler = new HttpClientHandler();
            return CreateHandlerPipeline(primatyHandler, _options.Value.Handlers);
        }
        
        private static HttpMessageHandler CreateHandlerPipeline(HttpMessageHandler primaryHandler, IEnumerable<DelegatingHandler> additionalHandlers)
        {
            // This is similar to https://github.com/aspnet/AspNetWebStack/blob/master/src/System.Net.Http.Formatting/HttpClientFactory.cs#L58
            // but we don't want to take that package as a dependency.

            if (primaryHandler == null)
            {
                throw new ArgumentNullException(nameof(primaryHandler));
            }

            if (additionalHandlers == null)
            {
                throw new ArgumentNullException(nameof(additionalHandlers));
            }

            var additionalHandlersList = additionalHandlers as IReadOnlyList<DelegatingHandler> ?? additionalHandlers.ToArray();

            var next = primaryHandler;
            for (var i = additionalHandlersList.Count - 1; i >= 0; i--)
            {
                var handler = additionalHandlersList[i];
                if (handler == null)
                {
                    throw new InvalidOperationException("There is no handler");
                }

                // Checking for this allows us to catch cases where someone has tried to re-use a handler. That really won't
                // work the way you want and it can be tricky for callers to figure out.
                if (handler.InnerHandler != null)
                {
                    throw new InvalidOperationException("Inner handler is not null");
                }

                handler.InnerHandler = next;
                next = handler;
            }

            return next;
        }
        
    }
}