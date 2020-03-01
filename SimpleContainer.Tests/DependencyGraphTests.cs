using NUnit.Framework;

using SimpleContainer.Tests.DummyTypes;

namespace SimpleContainer.Tests
{
    [TestFixture]
    public class DependencyGraphTests
    {
        [Test]
        public void Register_Pass()
        {
            var graph = new DependencyGraph(Container.Create());

            graph.Register<IPunk, Cyberpunk>(Scope.Singleton, null);
            graph.Register<ITechnology, TechnologyAI>(Scope.Singleton, null);

            graph.Link();
            graph.Resolve();

            Assert.Pass();
        }
    }
}