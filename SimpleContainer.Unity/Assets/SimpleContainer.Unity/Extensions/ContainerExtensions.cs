using System.Diagnostics.CodeAnalysis;

namespace SimpleContainer.Unity.Extensions
{
    public static class ContainerExtensions
    {
        [SuppressMessage("ReSharper", "UnusedTypeParameter")]
        public static void RegisterForAndroid<TContract, TResult>(this Container container, Scope scope)
            where TResult : TContract
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            container.Register<TContract, TResult>(scope);
#endif
        }

        public static void RegisterForEditor<TContract, TResult>(this Container container, Scope scope)
            where TResult : TContract
        {
#if UNITY_EDITOR
            container.Register<TContract, TResult>(scope);
#endif
        }
    }
}
