namespace SimpleContainer.Tests.DummyTypes
{
    public sealed class TechnologyAI : ITechnology
    {
        [InjectA]
        public IAIPart[] Parts { get; set; }
    }
}