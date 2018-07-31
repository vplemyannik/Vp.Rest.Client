using System;
using System.Collections.Generic;

namespace Vp.Rest.Client.Authentification
{
    public class AuthentificationBuilder
    {
        internal List<IAuthentificationOptions> AuthentificationOptionses { get; } = new List<IAuthentificationOptions>();

        public AuthentificationBuilder Basic(Action<BasicAuthentificationOptions> optionAction)
        {
            var createdOptions = new BasicAuthentificationOptions();
            optionAction(createdOptions);
            AuthentificationOptionses.Add(createdOptions);
            return this;
        }
    }

    public class BasicAuthentificationOptions : IAuthentificationOptions
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}