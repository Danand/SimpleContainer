namespace SimpleContainer.Tests.DummyTypes
{
    public interface ICustomEventHandler
    {
        CustomEventArgs ReceivedEventArgs { get; }
        void OnCustomEvent(CustomEventArgs eventArgs);
    }
}