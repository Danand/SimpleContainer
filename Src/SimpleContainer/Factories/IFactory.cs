using System;

namespace SimpleContainer.Factories
{
    public interface IFactory
    {
        SimpleContainer Container { get; set; }
        Type GetResultType(Type resultType, params object[] args);
    }
}