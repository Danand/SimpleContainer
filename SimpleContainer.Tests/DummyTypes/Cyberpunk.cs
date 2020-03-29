namespace SimpleContainer.Tests.DummyTypes
{
    public sealed class Cyberpunk : IPunk, ITrending
    {
        public ITechnology Technology { get; }

        public Cyberpunk(ITechnology technology)
        {
            Technology = technology;
        }
    }
}