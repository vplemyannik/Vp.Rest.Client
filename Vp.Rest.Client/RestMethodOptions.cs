using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Vp.Rest.Client
{
    public class RestMethodOptions
    {
        public List<DelegatingHandler> Handlers { get; } = new List<DelegatingHandler>();
        public string Url { get; internal set; }
        public TimeSpan  TimeOut { get; internal set; }
    }
}