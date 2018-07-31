using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Vp.Rest.Client.Authentification.DelegateConvertor
{
    public abstract class DefaultAuthentificationDelegatingConvertor<TOptions> : IAuthentificationDelegatingConvertor
    {
        public DelegatingHandler Convert(IEnumerable<IAuthentificationOptions> authentificationOptionses)
        {
            foreach (var option in authentificationOptionses)
            {
                if (option is TOptions castedOption)
                {
                    return Convert(castedOption);
                }
            }

            return null;
        }

        protected abstract DelegatingHandler Convert(TOptions options);
    }
}