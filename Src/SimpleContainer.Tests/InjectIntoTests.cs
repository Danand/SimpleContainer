using NUnit.Framework;

using SimpleContainer.Tests.DummyTypes;

namespace SimpleContainer.Tests
{
    /// <summary>
    /// Injection test in cases of member injection (except constructor).
    /// </summary>
    [TestFixture]
    public class InjectIntoTests
    {
        [Test]
        public void Inject_IntoField_Public()
        {
            var container = Container.Create();

            container.Register<IPhysics, PhysicsPlanetEarth>(Scope.Singleton);
            container.Register<IEngine, EngineMedium>(Scope.Transient);

            var expected = container.Resolve<IPhysics>();
            var engine = container.Resolve<IEngine>();

            Assert.AreSame(expected, engine.PhysicsFromFieldPublic);
        }

        [Test]
        public void Inject_IntoField_Private()
        {
            var container = Container.Create();

            container.Register<IPhysics, PhysicsPlanetEarth>(Scope.Singleton);
            container.Register<IEngine, EngineMedium>(Scope.Transient);

            var expected = container.Resolve<IPhysics>();
            var engine = container.Resolve<IEngine>();

            Assert.AreSame(expected, engine.PhysicsFromFieldPrivate);
        }

        [Test]
        public void Inject_IntoField_Private_ExistingInstance()
        {
            var container = Container.Create();
            var physics = new PhysicsPlanetEarth();

            container.Register<IPhysics>(Scope.Singleton, physics);
            container.Register<IEngine, EngineMedium>(Scope.Transient);

            var expected = container.Resolve<IPhysics>();
            var engine = container.Resolve<IEngine>();

            Assert.AreSame(expected, engine.PhysicsFromFieldPrivate);
        }

        [Test]
        public void Inject_IntoProperty_Public()
        {
            var container = Container.Create();

            container.Register<IPhysics, PhysicsPlanetEarth>(Scope.Singleton);
            container.Register<IEngine, EngineMedium>(Scope.Transient);

            var expected = container.Resolve<IPhysics>();
            var engine = container.Resolve<IEngine>();

            Assert.AreSame(expected, engine.PhysicsFromPropertyPublic);
        }

        [Test]
        public void Inject_IntoProperty_Private()
        {
            var container = Container.Create();

            container.Register<IPhysics, PhysicsPlanetEarth>(Scope.Singleton);
            container.Register<IEngine, EngineMedium>(Scope.Transient);

            var expected = container.Resolve<IPhysics>();
            var engine = container.Resolve<IEngine>();

            Assert.AreSame(expected, engine.PhysicsFromPropertyPrivate);
        }

        [Test]
        public void Inject_IntoMethod_Public()
        {
            var container = Container.Create();

            container.Register<IPhysics, PhysicsPlanetEarth>(Scope.Singleton);
            container.Register<IEngine, EngineMedium>(Scope.Transient);

            var expected = container.Resolve<IPhysics>();
            var engine = container.Resolve<IEngine>();

            Assert.AreSame(expected, engine.PhysicsFromMethodPublic);
        }

        [Test]
        public void Inject_IntoMethod_Private()
        {
            var container = Container.Create();

            container.Register<IPhysics, PhysicsPlanetEarth>(Scope.Singleton);
            container.Register<IEngine, EngineMedium>(Scope.Transient);

            var expected = container.Resolve<IPhysics>();
            var engine = container.Resolve<IEngine>();

            Assert.AreSame(expected, engine.PhysicsFromMethodPrivate);
        }
    }
}