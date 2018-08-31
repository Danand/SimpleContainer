namespace SimpleContainer.Tests.DummyTypes
{
    public class DummyInvoker
    {
        private readonly Dispatcher dispatcher;

        public DummyInvoker(Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        public void RaiseEvent(CustomArgs eventArgs)
        {
            dispatcher.Send(eventArgs);
        }
    }
}