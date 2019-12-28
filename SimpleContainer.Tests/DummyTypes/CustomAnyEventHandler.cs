using System;

using SimpleContainer.Interfaces;

namespace SimpleContainer.Tests.DummyTypes
{
    public class CustomAnyEventHandler : IEventHandlerAny
    {
        public CustomContractArgs ReceivedArgs { get; private set; }
        public Type EventType { get; private set; }

        void IEventHandlerAny.OnEvent(AnyArgs eventArgs)
        {
            ReceivedArgs = eventArgs.GetInnerArgs<CustomContractArgs>();
            EventType = eventArgs.GetInnerType();
        }
    }
}