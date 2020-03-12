namespace SimpleContainer.Tests.DummyTypes
{
    public sealed class TechnologyAI : ITechnology
    {
        [Inject]
        public IAIPart[] Parts { get; set; }
    }
}