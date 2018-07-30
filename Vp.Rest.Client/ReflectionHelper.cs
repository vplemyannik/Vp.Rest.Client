using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Vp.Rest.Client
{
    public class ReflectionHelper
    {
        private static MethodInfo _method => typeof(ReflectionHelper).GetMethod(nameof(CreateCompletionTaskSource));

        public ITaskCompletionSource CreateCompletionTaskSourceForType(Type type)
        {
            var method = _method.MakeGenericMethod(type);
            return (ITaskCompletionSource) method.Invoke(null, null);
        }

        private static ITaskCompletionSource CreateCompletionTaskSource<T>()
        {
            return new TaskComplitionWrapper<T>();
        }
    }

    public class TaskComplitionWrapper<T> : ITaskCompletionSource
    {
        private readonly TaskCompletionSource<T> _tcs;

        public TaskComplitionWrapper()
        {
            _tcs = new TaskCompletionSource<T>();
        }

        public Task Task => _tcs.Task;
        public void SetResult(object result) => _tcs.SetResult((T)result);
        public void SetException(Exception exception) => _tcs.SetException(exception);
        public void SetCancelled() => _tcs.SetCanceled();
    }

    public interface ITaskCompletionSource
    {
         Task Task { get; }
         void SetResult(object result);
         void SetException(Exception exception);
         void SetCancelled();
    }
}