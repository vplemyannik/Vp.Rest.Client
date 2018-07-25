using System;

namespace Vp.Rest.Client
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RestAttribute : Attribute
    {
        public RestMethod Method { get; }
        public string TemplatePath { get; }
        
        public RestAttribute(RestMethod method, string templatePath)
        {
            Method = method;
            TemplatePath = templatePath;
        }
    }
}