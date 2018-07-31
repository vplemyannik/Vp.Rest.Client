using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Vp.Rest.Client.Authentification.DelegateConvertor
{
    public class BasicAuthentificationDelegatingConvertor : DefaultAuthentificationDelegatingConvertor<BasicAuthentificationOptions>
    {
        protected override DelegatingHandler Convert(BasicAuthentificationOptions options)
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
            
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", credentials);
            return base.SendAsync(request, cancellationToken);
        }
    }
}