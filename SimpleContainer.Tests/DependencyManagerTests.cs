using System.Diagnostics;

using NUnit.Framework;

using SimpleContainer.Exceptions;
using SimpleContainer.Tests.DummyTypes;

namespace SimpleContainer.Tests
{
    [TestFixture]
    public class DependencyManagerTests
    {
        [Test]
        public void Link_Pass()
        {
            Container container = Container.Create();

            container.RegisterAttribute<InjectAttribute>();

            var graph = new DependencyManager(container);

            graph.Register<IPunk, Cyberpunk>(Scope.Singleton, null);
            graph.Register<ITechnology, TechnologyAI>(Scope.Singleton, null);
            graph.Register<IAIPart, AIPartConsciousness>(Scope.Singleton, null);
            graph.Register<IAIPart, AIPartSemiconduction>(Scope.Singleton, null);
            graph.Register<INeural, NeuralComputer>(Scope.Singleton, null);

            Assert.Pass();
        }

        [Test]
        public void Link_Throws_CircularDependencyException()
        {
            Container container = Container.Create();

            container.RegisterAttribute<InjectAttribute>();

            var graph = new DependencyManager(container);

            graph.Register<IPunk, Cyberpunk>(Scope.Singleton, null);
            graph.Register<ITechnology, TechnologyAINetwork>(Scope.Singleton, null);
            graph.Register<IAIPart, AIPartConsciousness>(Scope.Singleton, null);
            graph.Register<IAIPart, AIPartSemiconduction>(Scope.Singleton, null);
            graph.Register<INeural, NeuralGlobalNetwork>(Scope.Singleton, null);

            var throws = Assert.Throws<CircularDependencyException>(graph.Link);

            Debug.WriteLine(throws.Message);
        }

        [Test]
        public void Resolve_NotNull()
        {
            Container container = Container.Create();

            container.RegisterAttribute<InjectAttribute>();

            var graph = new DependencyManager(container);

            graph.Register<IPunk, Cyberpunk>(Scope.Singleton, null);
            graph.Register<ITechnology, TechnologyAI>(Scope.Singleton, null);
            graph.Register<IAIPart, AIPartConsciousness>(Scope.Singleton, null);
            graph.Register<IAIPart, AIPartSemiconduction>(Scope.Singleton, null);
            graph.Register<INeural, NeuralComputer>(Scope.Singleton, null);

            var actual = graph.Resolve(typeof(IPunk));

            Assert.IsNotNull(actual);
        }
    }
}