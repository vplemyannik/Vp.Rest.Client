using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Vp.Rest.Client.Models;

namespace Vp.Rest.Client
{
    public static class UriBuilder
    {
        private const char BR_RIGHT = '}';
        private const char BR_LEFT = '{';
        
        public static string Buld(string url, IEnumerable<Parameter> parameters)
        {
            var templateBuilder = new StringBuilder(32);
            var urlTemplate = new StringBuilder(url);
            var flag = State.NoAction;
            
            foreach (var character in url)
            {
                switch (character)
                {
                    case BR_LEFT:
                        flag = State.Start;
                        break;
                    case BR_RIGHT:
                        flag = State.End;
                        goto default;
                    default:
                        if (flag == State.Start)
                        {
                            templateBuilder.Append(character);
                        }

                        if (flag == State.End)
                        {
                            var param = parameters
                                    .FirstOrDefault(
                                        p => string.Equals(
                                            p.Name,
                                            templateBuilder.ToString(), 
                                            StringComparison.OrdinalIgnoreCase)
                                    );

                            var key = $"{{{param.Name}}}";
                            urlTemplate.Replace(key, param.Value.ToString());
                            templateBuilder.Clear();
                            flag = State.NoAction;
                        }
                        break;
                }
            }

            return urlTemplate.ToString();
        }
    }

    internal enum State
    {
        Start,
        End,
        NoAction
    }
}