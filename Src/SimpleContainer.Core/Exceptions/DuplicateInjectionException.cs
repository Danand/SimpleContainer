using System;

namespace SimpleContainer.Exceptions
{
    public class DuplicateInjectionException : Exception
    {
        public DuplicateInjectionException(object instance) : base($"Members of instance '{instance?.GetHashCode()}' already injected!") { }
    }
}