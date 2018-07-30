using System;

namespace Vp.Rest.Client
{
    [AttributeUsage(AttributeTargets.Interface)]
    public class RestContract : Attribute
    {
        public string BaseUrl { get;  }

        public RestContract(string url)
        {
            BaseUrl = url;
            BaseUrl = url;
        }
    }
}