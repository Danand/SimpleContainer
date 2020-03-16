namespace SimpleContainer.Tests.DummyTypes
{
    public sealed class TechnologyAINetwork : ITechnology
    {
        private readonly IAIPart part;

        public TechnologyAINetwork(IAIPart part)
        {
            this.part = part;
        }
    }
}