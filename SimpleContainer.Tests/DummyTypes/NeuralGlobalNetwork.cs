namespace SimpleContainer.Tests.DummyTypes
{
    public sealed class NeuralGlobalNetwork : INeural
    {
        private readonly IPunk punk;

        public NeuralGlobalNetwork(IPunk punk)
        {
            this.punk = punk;
        }
    }
}