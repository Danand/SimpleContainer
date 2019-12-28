using System.Reflection;

using SimpleContainer.Interfaces;

namespace SimpleContainer.Activators
{
    internal sealed class ActivatorReflection : IActivator
    {
        object IActivator.CreateInstance(ConstructorInfo constructor, object[] args)
        {
            return constructor.Invoke(args);
        }
    }
}
