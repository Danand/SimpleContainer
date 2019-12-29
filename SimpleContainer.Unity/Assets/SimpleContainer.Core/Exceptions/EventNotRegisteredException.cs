using System;

namespace SimpleContainer.Exceptions
{
    public class EventNotRegisteredException : Exception
    {
        public EventNotRegisteredException(Type type) : base($"Event type '{type.Name}' is not registered!") { }
    }
}