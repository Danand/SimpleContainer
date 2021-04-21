using System.Reflection;

namespace SimpleContainer.Interfaces
{
    public interface IActivator
    {
        object CreateInstance(ConstructorInfo constructor, object[] args);
    }
}