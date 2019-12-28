using System;

namespace SimpleContainer.Exceptions
{
    public sealed class CircularDependencyException : Exception
    {
        public CircularDependencyException(Type type, string bindings) : base($"Contract type '{type.Name}' has circular dependency!{Environment.NewLine}Bindings:{Environment.NewLine}{bindings}") { }
    }
}