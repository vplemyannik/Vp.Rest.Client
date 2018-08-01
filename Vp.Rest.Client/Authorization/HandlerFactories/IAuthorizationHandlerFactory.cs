using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Vp.Rest.Client.Authorization.HandlerFactories
{
    public interface IAuthorizationHandlerFactory
    {
        DelegatingHandler CreateHandler(IEnumerable<IAuthentificationOptions> authentificationOptionses);
    }
}