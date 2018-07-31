using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Vp.Rest.Client.Authentification.DelegateConvertor
{
    public interface IAuthentificationDelegatingConvertor
    {
        DelegatingHandler Convert(IEnumerable<IAuthentificationOptions> authentificationOptionses);
    }
}