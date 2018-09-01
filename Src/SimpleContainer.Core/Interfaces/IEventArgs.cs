using System;

namespace SimpleContainer.Interfaces
{
    public interface IEventArgs
    {
        Type HandlerType { get; }
    }
}