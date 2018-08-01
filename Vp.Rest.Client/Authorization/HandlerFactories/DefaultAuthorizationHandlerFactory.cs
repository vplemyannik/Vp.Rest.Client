using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Vp.Rest.Client.Authorization.HandlerFactories
{
    public abstract class DefaultAuthorizationHandlerFactory<TOptions> : IAuthorizationHandlerFactory
    {
        public DelegatingHandler CreateHandler(IEnumerable<IAuthentificationOptions> authentificationOptionses)
        {
            foreach (var option in authentificationOptionses)
            {
                if (option is TOptions castedOption)
                {
                    return CreateHandler(castedOption);
                }
            }

            return null;
        }

        protected abstract DelegatingHandler CreateHandler(TOptions options);
    }
}