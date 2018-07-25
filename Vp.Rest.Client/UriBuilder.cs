using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Vp.Rest.Client
{
    public static class UriBuilder
    {
        public static string Buld(string url, IDictionary<string, object> parameterValueMap)
        {
            var templateBuilder = new StringBuilder(32);
            var urlTemplate = new StringBuilder(url);
            var flag = State.NoAction;
            
            foreach (var character in url)
            {
                switch (character)
                {
                    case '{':
                        flag = State.Start;
                        break;
                    case '}':
                        flag = State.End;
                        goto default;
                    default:
                        if (flag == State.Start)
                        {
                            templateBuilder.Append(character);
                        }

                        if (flag == State.End)
                        {
                            var param = parameterValueMap
                                    .FirstOrDefault(
                                        p => string.Equals(
                                            p.Key,
                                            templateBuilder.ToString(), 
                                            StringComparison.OrdinalIgnoreCase)
                                    );

                            var key = $"{{{param.Key}}}";
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