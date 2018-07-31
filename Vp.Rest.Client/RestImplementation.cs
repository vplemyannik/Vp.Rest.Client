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
        private readonly Func<IInterceptor> _interceptorCreator;

        internal RestImplementation(Func<IInterceptor> interceptorCreator)
        {
            _interceptorCreator = interceptorCreator;
        }

        public T Create<T>()
        {
            var restMethodInspector = _interceptorCreator();
            var proxy = (T) _generator.CreateInterfaceProxyWithoutTarget(typeof(T), restMethodInspector);
            return proxy;
        }
        
        public object Create(Type type)
        {
            var restMethodInspector = _interceptorCreator();
            var proxy = _generator.CreateInterfaceProxyWithoutTarget(type, restMethodInspector);
            return proxy;
        }
    }
}