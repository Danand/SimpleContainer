using System;

using SimpleContainer.Interfaces;

namespace SimpleContainer
{
    internal class DynamicEventHandler<TEventArgs> : IEventHandler<TEventArgs>
        where TEventArgs : IEventArgs
    {
        private readonly Action<TEventArgs> callback;

        private Action<Action<object>> remove;

        public DynamicEventHandler() { }

        public DynamicEventHandler(Action<TEventArgs> callback)
        {
            this.callback = callback;
        }

        public bool IsCompleted { get; private set; }
        public TEventArgs Result { get; private set; }

        void IEventHandler<TEventArgs>.OnEvent(TEventArgs eventArgs)
        {
            IsCompleted = true;
        }

        public void Handle(object args)
        {
            Result = (TEventArgs)args;
            callback?.Invoke(Result);
            remove?.Invoke(Handle);
        }

        public void SubscribeOnce(Action<Action<object>> add, Action<Action<object>> remove)
        {
            add(Handle);
            this.remove = remove;
        }
    }
}