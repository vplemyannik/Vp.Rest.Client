using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Vp.RestClient.Content
{
    public class XmlContentManger : IContentManager
    {
        private string ContentType => "application/xml";

        public HttpContent CreateContent(object content)
        {
            using (var writer = new StringWriter())
            {
                using (var xmlWriter = new XmlTextWriter(writer))
                {
                    var serializer = new XmlSerializer(content.GetType());
                    serializer.Serialize(xmlWriter, content);
                    return new StringContent(writer.ToString(), Encoding.UTF8, ContentType);
                }
            }
        }

        public async Task<object> ReadContent(HttpContent content, Type resultType)
        {
            var xml = await content.ReadAsStringAsync();
            using (var reader = new StreamReader(xml))
            {
                using (var xmlReader = new XmlTextReader(reader))
                {
                    var serializer = new XmlSerializer(resultType);
                    return serializer.Deserialize(xmlReader);
                }
            }
        }
    }
}