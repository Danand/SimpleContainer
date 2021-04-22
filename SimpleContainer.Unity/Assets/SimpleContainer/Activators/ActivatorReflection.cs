using System.Reflection;

using SimpleContainer.Interfaces;

namespace SimpleContainer.Activators
{
    public sealed class ActivatorReflection : IActivator
    {
        object IActivator.CreateInstance(ConstructorInfo constructor, object[] args)
        {
            return constructor.Invoke(args);
        }
    }
}
