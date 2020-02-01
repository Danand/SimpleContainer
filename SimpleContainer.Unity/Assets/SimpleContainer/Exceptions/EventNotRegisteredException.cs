using System;

namespace SimpleContainer.Exceptions
{
    public sealed class EventNotRegisteredException : Exception
    {
        public EventNotRegisteredException(Type type) : base($"Event type '{type.Name}' is not registered!") { }
    }
}