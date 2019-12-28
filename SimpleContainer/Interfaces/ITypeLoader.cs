using System;

namespace SimpleContainer.Interfaces
{
    internal interface ITypeLoader
    {
        Type Load(string typeName);
    }
}