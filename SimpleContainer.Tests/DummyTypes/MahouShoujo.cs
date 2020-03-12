namespace SimpleContainer.Tests.DummyTypes
{
    public sealed class MahouShoujo
    {
        [InjectA]
        public IMagic MagicFirst;

        [InjectOther]
        public IMagic MagicSecond;
    }
}