namespace SimpleContainer.Tests.DummyTypes
{
    public class CustomEventHandler : ICustomEventHandler
    {
        public CustomEventArgs ReceivedEventArgs { get; private set; }

        public void OnCustomEvent(CustomEventArgs eventArgs)
        {
            ReceivedEventArgs = eventArgs;
        }
    }
}