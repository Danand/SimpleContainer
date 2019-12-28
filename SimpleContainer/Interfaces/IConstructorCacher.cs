using System;
using System.Reflection;

namespace SimpleContainer.Interfaces
{
    public interface IConstructorCacher
    {
        ConstructorInfo GetConstructor(Type type);
    }
}