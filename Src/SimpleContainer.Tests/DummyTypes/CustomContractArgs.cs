using SimpleContainer.Interfaces;

namespace SimpleContainer.Tests.DummyTypes
{
    public class CustomContractArgs : IEventArgs
    {
        public interface ICustomHandler : IEventHandler<CustomContractArgs> { }

        public bool flag;
        public int id;
        public string name;
    }
}