namespace SimpleContainer.Interfaces
{
    public interface IEventHandler<in TEventArgs>
        where TEventArgs : IEventArgs
    {
        void OnEvent(TEventArgs eventArgs);
    }
}