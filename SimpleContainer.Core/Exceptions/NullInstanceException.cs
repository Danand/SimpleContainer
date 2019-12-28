using System;

namespace SimpleContainer.Exceptions
{
    public class NullInstanceException : Exception
    {
        public NullInstanceException(Type type) : base($"Given instance of type '{type.Name}' is null!") { }
    }
}