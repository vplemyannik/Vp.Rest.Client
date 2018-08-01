using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Vp.Rest.Client
{
    public class RequestLoggingHandler : DelegatingHandler
    {
        private readonly ILogger _logger;
        
        public RequestLoggingHandler(ILogger logger)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var requestString = request.Content == null
                ? null
                : await request.Content
                    .ReadAsStringAsync()
                    .ConfigureAwait(false);

            var stopwatch = Stopwatch.StartNew();
            var requestTimestamp = DateTime.UtcNow;
            
            
            var responseMessage = await base.SendAsync(request, cancellationToken)
                .ConfigureAwait(false);
            
            var responseTimestamp = DateTime.UtcNow;
            stopwatch.Stop();

            string responseString = null;
            if (responseMessage.Content != null)
            {
                responseString = await responseMessage.Content
                    .ReadAsStringAsync()
                    .ConfigureAwait(false);
            }

            var stringBuilder = new StringBuilder(1024);
            stringBuilder.Append($"METHOD: {request.Method.Method}");
            stringBuilder.Append($"EXECUTION TIME: {stopwatch.Elapsed}");
            stringBuilder.AppendLine($"URI: {request.RequestUri}");
            stringBuilder.AppendLine($"REQUEST HEADERS: ");
            foreach (var header in GetHeaders(request.Headers))
            {
                stringBuilder.AppendLine("\t" + header.Key + " : ");
                stringBuilder.Append(string.Join(", ", header.Values));
                
            }
            stringBuilder.AppendLine($"REQUEST: {requestString}");
            stringBuilder.AppendLine($"RESPONSE CODE: {responseMessage.StatusCode}");
            stringBuilder.AppendLine($"RESPONSE CODE: {responseMessage.StatusCode}");
            stringBuilder.AppendLine($"RESPONSE HEADERS: ");
            foreach (var header in GetHeaders(responseMessage.Headers))
            {
                stringBuilder.AppendLine("\t" + header.Key + " : ");
                stringBuilder.Append(string.Join(", ", header.Values));
                
            }

            _logger.Log(LogLevel.Information, stringBuilder.ToString());

            return responseMessage;
        }

        private static IReadOnlyCollection<HttpHeaderInfo> GetHeaders(HttpHeaders headers)
        {
            return headers.Select(header => new HttpHeaderInfo(header.Key, header.Value.ToArray()))
                .ToArray();
        }
    }
    
    public class HttpHeaderInfo
    {
        public string Key { get; }
        public IReadOnlyCollection<string> Values { get; }

        public HttpHeaderInfo(string key, IReadOnlyCollection<string> values)
        {
            Key = key;
            Values = values;
        }
    }
}