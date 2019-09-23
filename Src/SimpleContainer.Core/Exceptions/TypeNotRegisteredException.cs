using System;

namespace SimpleContainer.Exceptions
{
    public class TypeNotRegisteredException : Exception
    {
        public TypeNotRegisteredException(Type type, string bindings) : base($"Contract type '{type.Name}' is not registered!{Environment.NewLine}Bindings:{Environment.NewLine}{bindings}") { }
    }
}