using SimpleContainer.Interfaces;

namespace SimpleContainer
{
    internal class DynamicEventHandler<TEventArgs> : IEventHandler<TEventArgs>
        where TEventArgs : IEventArgs
    {
        public bool IsCompleted { get; private set; }

        void IEventHandler<TEventArgs>.OnEvent(TEventArgs eventArgs)
        {
            IsCompleted = true;
        }
    }
}