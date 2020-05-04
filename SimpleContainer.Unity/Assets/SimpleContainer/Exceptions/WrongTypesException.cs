using System;

namespace SimpleContainer.Exceptions
{
    public sealed class WrongTypesException : Exception
    {
        public WrongTypesException(Type contractType, Type resultType) : base($"Result type '{resultType.Name}' is not derived from contract type '{contractType.Name}'") { }
    }
}