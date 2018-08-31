namespace SimpleContainer.Tests.DummyTypes
{
    public interface ICustomHandler
    {
        CustomArgs ReceivedArgs { get; }
        void OnCustomEvent(CustomArgs eventArgs);
    }
}