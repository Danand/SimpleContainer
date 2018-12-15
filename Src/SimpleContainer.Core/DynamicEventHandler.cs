using SimpleContainer.Interfaces;

namespace SimpleContainer
{
    internal class DynamicEventHandler<TEventArgs> : IEventHandler<TEventArgs>
        where TEventArgs : IEventArgs
    {
        public bool IsCompleted { get; private set; }
        public TEventArgs Result { get; private set; }

        void IEventHandler<TEventArgs>.OnEvent(TEventArgs eventArgs)
        {
            IsCompleted = true;
        }

        public void Handle(object args)
        {
            Result = (TEventArgs)args;
        }
    }
}