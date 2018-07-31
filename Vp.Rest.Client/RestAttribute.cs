using System;

namespace Vp.Rest.Client
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RestAttribute : Attribute
    {
        public RestMethod Method { get; }
        public string TemplatePath { get; }
        public string ContetnType { get; }
        
        public RestAttribute(RestMethod method, string templatePath, string contentType = "application/json")
        {
            Method = method;
            TemplatePath = templatePath;
            ContetnType = contentType;
        }
    }
}