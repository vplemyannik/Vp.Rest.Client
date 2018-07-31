using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Castle.DynamicProxy;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Vp.Rest.Client.Content;
using Vp.Rest.Client.Models;

namespace Vp.Rest.Client
{
    public class RestMethodInterceptor : IRestMethodInterceptor
    {
        private readonly IOptions<RestMethodOptions> _options;

        private readonly IDictionary<RestMethod, HttpMethod> _httpMethodMap 
            = new Dictionary<RestMethod, HttpMethod>
        {
            {RestMethod.GET, HttpMethod.Get},
            {RestMethod.POST, HttpMethod.Post},
            {RestMethod.PUT, HttpMethod.Put},
            {RestMethod.DELETE, HttpMethod.Delete},
            {RestMethod.HEAD, HttpMethod.Head},
            {RestMethod.OPTION, HttpMethod.Options},
        };

        private readonly IDictionary<string, Lazy<IContentManager>> _contentFactory =
            new Dictionary<string, Lazy<IContentManager>>
            {
                { "application/json", new Lazy<IContentManager>(() => new JsonContentManager())},
                { "application/xml", new Lazy<IContentManager>(() => new XmlContentManger()) }
            };

        public RestMethodInterceptor(IOptions<RestMethodOptions> options)
        {
            _options = options;
        }

        public void Intercept(IInvocation invocation)
        {
            var method = invocation.GetConcreteMethod();
            var client = CreateHttpClient(invocation);

            var restAttribute = method.GetAttribute<RestAttribute>();
            
            var httpMethod = _httpMethodMap[restAttribute.Method];

            var parametersInfo = method.GetParameters();
            var parameters = parametersInfo
                .Zip(invocation.Arguments, 
                    (paramInfo, value) 
                        => new Parameter(paramInfo, value));


            var relativeUrl = UriBuilder.Buld(restAttribute.TemplatePath, parameters);

            var content = CreateHttpContent(parameters, restAttribute.ContetnType);

            var request = CreateHttpRequest(httpMethod, relativeUrl, content, null);
            Execute(invocation, client, request);

        }

        private HttpRequestMessage CreateHttpRequest( 
            HttpMethod method, 
            string relativeUrl, 
            HttpContent content,
            IEnumerable<KeyValuePair<string, string>> headers)
        {
            var request = new HttpRequestMessage(method, _options.Value.Url + relativeUrl);
            
            foreach (var header in headers ?? Enumerable.Empty<KeyValuePair<string, string>>())
            {
                request.Headers.Add(header.Key, header.Value);
            }

            if (method != HttpMethod.Get)
            {
                request.Content = content;
            }

            return request;
        }

        private void Execute(IInvocation invocation, HttpClient client, HttpRequestMessage requestMessage)
        {
            var restMethodInfo = invocation.GetConcreteMethod();
            Task<HttpResponseMessage> task = null;
            if (restMethodInfo.ReturnType == typeof(Task))
            {
                task = client.SendAsync(requestMessage);
                invocation.ReturnValue = task;
            }

            else if(typeof(Task).IsAssignableFrom(restMethodInfo.ReturnType))
            {
                task = client.SendAsync(requestMessage);
                var unwrapType = restMethodInfo.ReturnType.GetGenericArguments()[0];
                var completion = ReflectionHelper.CreateCompletionTaskSourceForType(unwrapType);
                task.ContinueWith(currentTask =>
                {
                    if (currentTask.IsFaulted)
                    {
                        completion.SetException(currentTask.Exception);
                    }

                    if (currentTask.Status == TaskStatus.RanToCompletion)
                    {
                        var content = currentTask.Result.Content;
                        var contentManger = _contentFactory[content.Headers.ContentType.MediaType];
                        var responseTask = contentManger.Value.ReadContent(content, unwrapType);
                        responseTask.ContinueWith(readTask =>
                        {
                            if (readTask.IsFaulted)
                            {
                                completion.SetException(readTask.Exception);
                            }

                            completion.SetResult(readTask.Result);
                        });
                    }
                });
                
                invocation.ReturnValue = completion.Task;
            }
           
        }

        private HttpClient CreateHttpClient(IInvocation invocation)
        {
            return new HttpClient(CreateHttpMessageHandler(invocation));
        }
        
        private HttpMessageHandler CreateHttpMessageHandler(IInvocation invocation)
        {
            var primatyHandler = new HttpClientHandler();
            if(_options.Value.Handlers == null || !_options.Value.Handlers.Any())
                return new HttpClientHandler();
            return CreateHandlerPipeline(primatyHandler, _options.Value.Handlers);
        }

        private HttpContent CreateHttpContent(IEnumerable<Parameter> parameters, string contentType)
        {
            var bodyParameter = parameters
                .FirstOrDefault(p => p.ParameterInfo.GetCustomAttribute<Body>() != null);
            
            if(bodyParameter == null)
                return null;

            var contentManager = _contentFactory[contentType];

            return contentManager.Value.CreateContent(bodyParameter.Value);
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