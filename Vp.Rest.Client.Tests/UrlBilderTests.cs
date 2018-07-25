using System.Collections.Generic;
using NUnit.Framework;

namespace Vp.Rest.Client.Tests
{
    [TestFixture]
    public class UrlBilderTests
    {

        [Test]
        public void BuildUrl_Success()
        {
            //Arrange
            var testUrl = "api/orders/{orderId}/products/?orderby={orderQuery}";
            var exceptedUrl = "api/orders/12345/products/?orderby=price";

            var dictionary = new Dictionary<string, object>
            {
                {"orderId", 12345},
                {"orderQuery", "price"}
            };
            
            var result = UriBuilder.Buld(testUrl, dictionary);
            
            Assert.AreEqual(exceptedUrl, result);
        }
    }
}