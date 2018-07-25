using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Vp.Rest.Client
{
    public class RestMethodOptions
    {
        public IList<DelegatingHandler> Handlers { get; }
        public string Url { get; internal set; }
        public TimeSpan  TimeOut { get; internal set; }
    }
}