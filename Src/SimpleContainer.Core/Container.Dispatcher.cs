using System;

namespace SimpleContainer
{
    public sealed partial class Container
    {
        public void RegisterEvent<TEventHandler, TEventArgs>(Action<TEventHandler, TEventArgs> action)
        {
            dispatcher.RegisterEvent(this, action);
        }

        public void SendEvent<TEventArgs>(TEventArgs eventArgs)
        {
            dispatcher.Send(eventArgs);
        }
    }
}