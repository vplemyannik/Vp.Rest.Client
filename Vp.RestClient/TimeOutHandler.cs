using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Vp.RestClient
{
    public class TimeOutHandler : DelegatingHandler
    {
        public TimeOutHandler(TimeSpan timeOut)
        {
            TimeOut = timeOut;
        }

        private TimeSpan TimeOut { get; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            using (var cts = CreateCancellationToken(TimeOut, cancellationToken))
            {
                return base.SendAsync(request, cts.Token);
            }
        }

        private CancellationTokenSource CreateCancellationToken(TimeSpan timeOut, CancellationToken defaultToken)
        {
            var timeOutToken = new CancellationTokenSource(timeOut).Token;
            return CancellationTokenSource.CreateLinkedTokenSource(timeOutToken, defaultToken);
        }
    }
}