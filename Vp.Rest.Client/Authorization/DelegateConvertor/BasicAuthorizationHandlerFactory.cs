using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Vp.Rest.Client.Authorization.DelegateConvertor
{
    public class BasicAuthorizationHandlerFactory : DefaultAuthorizationHandlerFactory<BasicAuthentificationOptions>
    {
        protected override DelegatingHandler CreateHandler(BasicAuthentificationOptions options)
        {
            return new BasicDelegatingHandler(options);
        }
    }

    public class BasicDelegatingHandler : DelegatingHandler
    {
        private BasicAuthentificationOptions _authentificationOptions;

        public BasicDelegatingHandler(BasicAuthentificationOptions authentificationOptions)
        {
            _authentificationOptions = authentificationOptions;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var credentials = Convert.ToBase64String(
                Encoding.UTF8.GetBytes(
                    $"{_authentificationOptions.UserName}:{_authentificationOptions.Password}"
                    )
                );
            
            request.Headers.Authorization = new AuthenticationHeaderValue(_authentificationOptions.Shema, credentials);
            return base.SendAsync(request, cancellationToken);
        }
    }
}