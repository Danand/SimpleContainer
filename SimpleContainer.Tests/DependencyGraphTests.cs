using System;

using NUnit.Framework;

using SimpleContainer.Exceptions;
using SimpleContainer.Tests.DummyTypes;

namespace SimpleContainer.Tests
{
    [TestFixture]
    public class DependencyGraphTests
    {
        [Test]
        public void Link_Pass()
        {
            Container container = Container.Create();

            container.RegisterAttribute<InjectAAttribute>();

            var graph = new DependencyGraph(container);

            graph.Register<IPunk, Cyberpunk>(Scope.Singleton, null);
            graph.Register<ITechnology, TechnologyAI>(Scope.Singleton, null);
            graph.Register<IAIPart, AIPartConsciousness>(Scope.Singleton, null);
            graph.Register<IAIPart, AIPartSemiconduction>(Scope.Singleton, null);
            graph.Register<INeural, NeuralComputer>(Scope.Singleton, null);

            graph.Link();

            Assert.Pass();
        }

        [Test]
        public void Link_Throws_CircularDependencyException()
        {
            Container container = Container.Create();

            container.RegisterAttribute<InjectAAttribute>();

            var graph = new DependencyGraph(container);

            graph.Register<IPunk, Cyberpunk>(Scope.Singleton, null);
            graph.Register<ITechnology, TechnologyAINetwork>(Scope.Singleton, null);
            graph.Register<IAIPart, AIPartConsciousness>(Scope.Singleton, null);
            graph.Register<IAIPart, AIPartSemiconduction>(Scope.Singleton, null);
            graph.Register<INeural, NeuralGlobalNetwork>(Scope.Singleton, null);

            Assert.Throws<CircularDependencyException>(graph.Link);
        }
    }
}