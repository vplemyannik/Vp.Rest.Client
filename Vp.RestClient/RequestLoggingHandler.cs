using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Vp.RestClient
{
    public class RequestLoggingHandler : DelegatingHandler
    {
        private readonly Func<HttpRequestMessage, Task> _logRequest;
        private readonly Func<HttpResponseMessage, Task> _logResponse;
        
        public RequestLoggingHandler(Func<HttpRequestMessage, Task> logRequest, Func<HttpResponseMessage, Task> logResponse)
        {
            _logRequest = logRequest;
            _logResponse = logResponse;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            await _logRequest(request);
            var responseMessage = await base.SendAsync(request, cancellationToken);
            await _logResponse(responseMessage);
            return responseMessage;
        }
    }

}