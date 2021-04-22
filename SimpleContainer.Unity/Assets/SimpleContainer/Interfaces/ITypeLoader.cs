using System;

namespace SimpleContainer.Interfaces
{
    public interface ITypeLoader
    {
        Type Load(string typeName);
    }
}