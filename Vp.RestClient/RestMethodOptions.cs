using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Vp.RestClient
{
    internal class RestMethodOptions
    {
        public List<DelegatingHandler> Handlers { get; } = new List<DelegatingHandler>();
        public HttpClient HttpClient { get; internal set; } 
        public string Url { get; internal set; }
        public TimeSpan  TimeOut { get; internal set; } = TimeSpan.FromSeconds(100);
    }
}