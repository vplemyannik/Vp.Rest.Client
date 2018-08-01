using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Vp.Rest.Client.Builders
{
    public static class HttpClientBuilder
    {
        public static HttpClient Build(IEnumerable<DelegatingHandler> handlers)
        {
            return new HttpClient(CreateHandlerPipeline(new HttpClientHandler(), handlers));
        }
        
        private static HttpMessageHandler CreateHandlerPipeline(HttpMessageHandler primaryHandler, IEnumerable<DelegatingHandler> additionalHandlers)
        {
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