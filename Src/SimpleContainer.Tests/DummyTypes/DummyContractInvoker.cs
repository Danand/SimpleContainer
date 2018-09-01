namespace SimpleContainer.Tests.DummyTypes
{
    public class DummyContractInvoker
    {
        private readonly Dispatcher dispatcher;

        public DummyContractInvoker(Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        public void RaiseEvent(CustomContractArgs eventArgs)
        {
            dispatcher.Send(eventArgs);
        }
    }
}