using System;

namespace SimpleContainer.Exceptions
{
    public sealed class ConstructorNotFoundException : Exception
    {
        public ConstructorNotFoundException(Type type) : base($"Result type '{type.Name}' does not contain any public constructors!") { }
    }
}