using System.Reflection;

namespace SimpleContainer.Interfaces
{
    internal interface IActivator
    {
        object CreateInstance(ConstructorInfo constructor, object[] args);
    }
}