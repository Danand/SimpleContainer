namespace SimpleContainer.Tests.DummyTypes
{
    public sealed class AIPartConsciousness : IAIPart
    {
        private readonly INeural neural;

        public AIPartConsciousness(INeural neural)
        {
            this.neural = neural;
        }
    }
}