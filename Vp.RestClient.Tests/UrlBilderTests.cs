using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Vp.RestClient.Models;

namespace Vp.RestClient.MsTests
{
    [TestClass]
    public class UrlBilderTests
    {
        [TestMethod]
        public void BuildUrl_Success()
        {
            //Arrange
            var testUrl = "api/orders/{orderId}/products/?orderby={orderQuery}";
            var exceptedUrl = "api/orders/12345/products/?orderby=price";

            var dictionary = new List<Parameter>
            {
                new Parameter("orderId", 12345),
                new Parameter("orderQuery", "price"),
            };
            
            var result = UriBuilder.Build(testUrl, dictionary);
            
            Assert.AreEqual(exceptedUrl, result);
        }
    }
}