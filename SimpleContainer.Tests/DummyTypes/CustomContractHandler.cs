using SimpleContainer.Interfaces;

namespace SimpleContainer.Tests.DummyTypes
{
    public class CustomContractHandler : IEventHandler<CustomContractArgs>
    {
        public CustomContractArgs ReceivedArgs { get; set; }

        void IEventHandler<CustomContractArgs>.OnEvent(CustomContractArgs eventArgs)
        {
            ReceivedArgs = eventArgs;
        }
    }
}