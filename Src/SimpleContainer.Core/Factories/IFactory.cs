using System;

namespace SimpleContainer.Factories
{
    public interface IFactory
    {
        Container Container { get; set; }
        Type GetResultType(Type resultType, params object[] args);
    }
}