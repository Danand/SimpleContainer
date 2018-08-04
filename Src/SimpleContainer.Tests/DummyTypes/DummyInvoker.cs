namespace SimpleContainer.Tests.DummyTypes
{
    public class DummyInvoker
    {
        private readonly Dispatcher dispatcher;

        public DummyInvoker(Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        public void RaiseEvent()
        {
            dispatcher.Send(new CustomEventArgs
            {
                flag = true,
                id = 9,
                name = "shine"
            });
        }
    }
}