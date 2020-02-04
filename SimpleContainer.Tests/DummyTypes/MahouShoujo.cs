namespace SimpleContainer.Tests.DummyTypes
{
    public sealed class MahouShoujo
    {
        [InjectA]
        public IMagic MagicFirst;

        [InjectB]
        public IMagic MagicSecond;
    }
}