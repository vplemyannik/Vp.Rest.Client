using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Vp.RestClient.Content
{
    public interface IContentManager
    {
        HttpContent CreateContent(object content);

        Task<object> ReadContent(HttpContent content, Type resultType);
    }
}