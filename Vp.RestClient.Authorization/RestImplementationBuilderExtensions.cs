using System;
using Vp.RestClient.Configuration;

namespace Vp.RestClient.Authorization
{
    public static class RestImplementationBuilderExtensions
    {
        public static RestImplementationBuilder AddBasicAuthorization(
            this RestImplementationBuilder builder, 
            string userName, 
            string password)
        {
            return builder.WithHandler(new BasicAuthorizationHandler(userName, password));
        }
    }
}