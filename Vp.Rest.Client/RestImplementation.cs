using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using Castle.DynamicProxy;

namespace Vp.Rest.Client
{
    public class RestImplementation
    {
        private static readonly ProxyGenerator _generator = new ProxyGenerator();
        
        public IList<DelegatingHandler> Handlers { get; }
        public string Url { get; internal set; }
        public TimeSpan  TimeOut { get; internal set; }

        public T Create<T>()
        {
            var restMethodInspector = new RestMethodInterceptor();
            var proxy = (T) _generator.CreateInterfaceProxyWithoutTarget(typeof(T), restMethodInspector);
            return proxy;
        }
    }
}