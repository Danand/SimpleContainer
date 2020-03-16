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

            container.RegisterAttribute<InjectAttribute>();

            container.Register<IPhysics, PhysicsPlanetEarth>(Scope.Singleton);
            container.Register<IEngine, EngineMedium>(Scope.Transient);

            var expected = container.Resolve<IPhysics>();
            var engine = container.Resolve<IEngine>();

            Assert.AreSame(expected, engine.PhysicsFromFieldPublic);
        }

        [Test]
        public void Inject_IntoProperty_Public()
        {
            var container = Container.Create();

            container.RegisterAttribute<InjectAttribute>();

            container.Register<IPhysics, PhysicsPlanetEarth>(Scope.Singleton);
            container.Register<IEngine, EngineMedium>(Scope.Transient);

            var expected = container.Resolve<IPhysics>();
            var engine = container.Resolve<IEngine>();

            Assert.AreSame(expected, engine.PhysicsFromPropertyPublic);
        }

        [Test]
        public void Inject_IntoProperty_Public_Attributes()
        {
            var container = Container.Create();

            container.RegisterAttribute<InjectAttribute>();
            container.RegisterAttribute<InjectOtherAttribute>();

            container.Register<MahouShoujo>(Scope.Singleton);
            container.Register<IMagic, MagicPink>(Scope.Singleton);
            container.Register<IMagic, MagicDark>(Scope.Singleton);

            var actual = container.Resolve<MahouShoujo>();

            Assert.NotNull(actual.MagicFirst);
            Assert.NotNull(actual.MagicSecond);
        }

        [Test]
        public void Inject_IntoProperty_Public_Multiple()
        {
            var container = Container.Create();

            container.RegisterAttribute<InjectAttribute>();

            container.Register<IPhysics, PhysicsPlanetEarth>(Scope.Singleton);
            container.Register<IPhysics, PhysicsPlanetMars>(Scope.Singleton);
            container.Register<IEngine, EngineMedium>(Scope.Transient);

            var engine = container.Resolve<IEngine>();

            Assert.IsTrue(engine.PhysicsFromMethodPublicMultiple.Length == 2);
        }

        [Test]
        public void Inject_IntoMethod_Public()
        {
            var container = Container.Create();

            container.RegisterAttribute<InjectAttribute>();

            container.Register<IPhysics, PhysicsPlanetEarth>(Scope.Singleton);
            container.Register<IEngine, EngineMedium>(Scope.Transient);

            var expected = container.Resolve<IPhysics>();
            var engine = container.Resolve<IEngine>();

            Assert.AreSame(expected, engine.PhysicsFromMethodPublic);
        }
    }
}