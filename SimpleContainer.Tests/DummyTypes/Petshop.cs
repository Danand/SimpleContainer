using SimpleContainer.Attributes;

namespace SimpleContainer.Tests.DummyTypes
{
    public sealed class Petshop
    {
        [Inject]
        public IPetFood Food { get; set; }
    }
}