namespace SimpleContainer.Tests.DummyTypes
{
    public sealed class ConstructorlessType
    {
        private ConstructorlessType() { }

        internal ConstructorlessType(int x) { }

        protected ConstructorlessType(float y) { }
    }
}