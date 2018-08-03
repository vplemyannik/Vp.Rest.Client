using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Vp.RestClient.Authorization
{
    public class BasicAuthorizationHandler : DelegatingHandler
    {
        private string UserName { get; }
        private string Password { get; }
            
        public BasicAuthorizationHandler(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var credentials = Convert.ToBase64String(
                Encoding.UTF8.GetBytes(
                    $"{UserName}:{Password}"
                )
            );
            
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", credentials);
            return base.SendAsync(request, cancellationToken);
        }
    }
}