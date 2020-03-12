using SimpleContainer.Tests.DummyTypes;

using NUnit.Framework;

namespace SimpleContainer.Tests
{
    /// <summary>
    /// Injection tests grouped by scope.
    /// Uses classic constructor injection in every case.
    /// </summary>
    [TestFixture]
    public class InjectScopeTests
    {
        [Test]
        public void Inject_Transient()
        {
            var container = Container.Create();

            container.RegisterAttribute<InjectAttribute>();

            container.Register<IEngine, EngineBig>(Scope.Transient);
            container.Register<ICar, CarFourWheelDrive>(Scope.Transient);

            var carFirst = container.Resolve<ICar>();
            var carSecond = container.Resolve<ICar>();

            Assert.AreNotSame(carFirst.Engine, carSecond.Engine);
        }

        [Test]
        public void Inject_Transient_All_Inline()
        {
            const int EXPECTED_COUNT = 2;

            var container = Container.Create();

            container.RegisterAttribute<InjectAttribute>();

            container.Register<IColor>(typeof(ColorRed), typeof(ColorBlue));
            container.Register<IColorPalette, ColorPalette>(Scope.Singleton);

            var palette = container.Resolve<IColorPalette>();
            var actualCount = palette.Colors.Length;

            Assert.AreEqual(EXPECTED_COUNT, actualCount);
        }

        [Test]
        public void Inject_Transient_All_Multiline()
        {
            const int EXPECTED_COUNT = 2;

            var container = Container.Create();

            container.RegisterAttribute<InjectAttribute>();

            container.Register<IColor, ColorRed>(Scope.Transient);
            container.Register<IColor, ColorBlue>(Scope.Transient);
            container.Register<IColorPalette, ColorPalette>(Scope.Singleton);

            var palette = container.Resolve<IColorPalette>();
            var actualCount = palette.Colors.Length;

            Assert.AreEqual(EXPECTED_COUNT, actualCount);
        }

        [Test]
        public void Inject_Singleton()
        {
            var container = Container.Create();

            container.RegisterAttribute<InjectAttribute>();

            container.Register<IPhysics, PhysicsPlanetEarth>(Scope.Singleton);
            container.Register<IEngine, EngineMedium>(Scope.Transient);

            var enigneFirst = container.Resolve<IEngine>();
            var enigneSecond = container.Resolve<IEngine>();

            Assert.AreSame(enigneFirst.Physics, enigneSecond.Physics);
        }
    }
}