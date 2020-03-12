namespace SimpleContainer.Tests.DummyTypes
{
    public sealed class MahouShoujo
    {
        [Inject]
        public IMagic MagicFirst;

        [InjectOther]
        public IMagic MagicSecond;
    }
}