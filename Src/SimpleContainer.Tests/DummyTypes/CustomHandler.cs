namespace SimpleContainer.Tests.DummyTypes
{
    public class CustomHandler : ICustomHandler
    {
        public CustomArgs ReceivedArgs { get; private set; }

        public void OnCustomEvent(CustomArgs eventArgs)
        {
            ReceivedArgs = eventArgs;
        }
    }
}