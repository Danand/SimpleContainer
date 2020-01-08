using System;
using System.Reflection;

namespace SimpleContainer.Interfaces
{
    internal interface IActivator
    {
        object CreateInstance(Type type);

        object CreateInstance(Type type, object[] args);

        object CreateInstance(ConstructorInfo constructor, object[] args);
    }
}