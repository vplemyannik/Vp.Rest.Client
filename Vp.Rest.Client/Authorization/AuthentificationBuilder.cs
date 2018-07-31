using System;
using System.Collections.Generic;

namespace Vp.Rest.Client.Authorization
{
    public class AuthentificationBuilder
    {
        internal List<IAuthentificationOptions> AuthentificationOptionses { get; } = new List<IAuthentificationOptions>();

        public void Basic(Action<BasicAuthentificationOptions> optionAction)
        {
            var createdOptions = new BasicAuthentificationOptions();
            optionAction(createdOptions);
            AuthentificationOptionses.Add(createdOptions);
        }
    }

    public class BasicAuthentificationOptions : IAuthentificationOptions
    {
        public string Shema => "Basic";
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}