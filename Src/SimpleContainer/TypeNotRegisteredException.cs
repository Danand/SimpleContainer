using System;

namespace SimpleContainer
{
    public class TypeNotRegisteredException : Exception
    {
        public TypeNotRegisteredException(Type type) : base($"Contract type '{type.Name}' is not registered!") { }
    }
}