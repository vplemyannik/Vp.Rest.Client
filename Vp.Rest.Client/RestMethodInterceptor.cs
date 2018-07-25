using Castle.DynamicProxy;

namespace Vp.Rest.Client
{
    public class RestMethodInterceptor : IRestMethodInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            var method = invocation.GetConcreteMethod();
            if (method.ReturnType == typeof(string))
            {
                invocation.ReturnValue = method.Name;
                return;
            }
        }
        
    }
}