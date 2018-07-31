using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Vp.Rest.Client.Content
{
    public class JsonContentManager : IContentManager
    {
        private string ContentType => "application/json"; 
        
        public HttpContent CreateContent(object content)
        {
            return new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, ContentType);
        }

        public async Task<object> ReadContent(HttpContent content, Type resultType)
        {
            var result = await content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject(result, resultType);
        }
    }
}