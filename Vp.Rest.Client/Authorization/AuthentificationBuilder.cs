using System;
using System.Collections.Generic;

namespace Vp.Rest.Client.Authorization
{
    public class AuthentificationBuilder
    {
        internal List<IAuthentificationOptions> AuthentificationOptions { get; } = new List<IAuthentificationOptions>();

        public void Basic(string userName, string password)
        {
            var createdOptions = new BasicAuthentificationOptions
            {
                UserName = userName,
                Password = password
            };
            AuthentificationOptions.Add(createdOptions);
        }
    }

    public class BasicAuthentificationOptions : IAuthentificationOptions
    {
        public string Shema => "Basic";
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}