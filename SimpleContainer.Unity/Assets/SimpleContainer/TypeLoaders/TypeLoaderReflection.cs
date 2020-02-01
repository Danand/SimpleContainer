using System;

using SimpleContainer.Interfaces;

namespace SimpleContainer.TypeLoaders
{
    internal sealed class TypeLoaderReflection : ITypeLoader
    {
        Type ITypeLoader.Load(string typeName)
        {
            throw new NotImplementedException();
        }
    }
}