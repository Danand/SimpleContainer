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
            Container container = Container.Create();

            container.RegisterAttribute<InjectAAttribute>();

            var graph = new DependencyGraph(container);

            graph.Register<IPunk, Cyberpunk>(Scope.Singleton, null);
            graph.Register<ITechnology, TechnologyAI>(Scope.Singleton, null);
            graph.Register<IAIPart, AIPartConsciousness>(Scope.Singleton, null);
            graph.Register<IAIPart, AIPartSemiconduction>(Scope.Singleton, null);

            graph.Link();

            Assert.Pass();
        }
    }
}