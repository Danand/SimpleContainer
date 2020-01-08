using System;

namespace SimpleContainer.Exceptions
{
    public class TypeNotResolvedException : Exception
    {
        public TypeNotResolvedException(Type type) : base($"Contract type '{type.Name}' is not resolved!") { }
    }
}