using SimpleContainer.Tests.DummyTypes;
using SimpleContainer.Exceptions;

using NUnit.Framework;

namespace SimpleContainer.Tests
{
    [TestFixture]
    public class ResolveTests
    {
        [Test]
        public void Resolve_Transient()
        {
            var container = Container.Create();

            container.Register<ICar, CarTruck>(Scope.Transient);

            var carFirst = container.Resolve<ICar>();
            var carSecond = container.Resolve<ICar>();

            Assert.AreNotSame(carFirst, carSecond);
        }

        [Test]
        public void Resolve_Transient_All_Generic()
        {
            const int EXPECTED_COUNT = 2;

            var container = Container.Create();

            container.Register<IColor>(typeof(ColorRed), typeof(ColorBlue));

            var colors = container.ResolveAll<IColor>();
            var actualCount = colors.Length;

            Assert.AreEqual(EXPECTED_COUNT, actualCount);
        }

        [Test]
        public void Resolve_Transient_All_Instances_Different()
        {
            const int EXPECTED_COUNT = 2;

            var container = Container.Create();

            var colorRed = new ColorRed();
            var colorBlue = new ColorBlue();

            container.Register<IColor, ColorRed>(Scope.Transient, colorRed);
            container.Register<IColor, ColorBlue>(Scope.Transient, colorBlue);

            var colors = container.ResolveAll<IColor>();
            var actualCount = colors.Length;

            Assert.AreEqual(EXPECTED_COUNT, actualCount);
            Assert.AreNotEqual(colors[0], colors[1]);
        }

        [Test]
        public void Resolve_Transient_All_Instances_Same()
        {
            const int EXPECTED_COUNT = 2;

            var container = Container.Create();

            var colorRedOne = new ColorRed();
            var colorRedTwo = new ColorRed();

            container.Register(Scope.Transient, colorRedOne);
            container.Register(Scope.Transient, colorRedTwo);

            var colors = container.ResolveAll<ColorRed>();
            var actualCount = colors.Length;

            Assert.AreEqual(EXPECTED_COUNT, actualCount);
            Assert.AreNotEqual(colors[0], colors[1]);
        }

        [Test]
        public void Resolve_Transient_All_NonGeneric_Explicit()
        {
            const int EXPECTED_COUNT = 2;

            var container = Container.Create();

            container.Register<IColor>(typeof(ColorRed), typeof(ColorBlue));

            var colors = container.ResolveAll(typeof(IColor));
            var actualCount = colors.Length;

            Assert.AreEqual(EXPECTED_COUNT, actualCount);
        }

        [Test]
        public void Resolve_Transient_All_NonGeneric_Implicit()
        {
            const int EXPECTED_COUNT = 2;

            var container = Container.Create();

            container.Register<IColor>(typeof(ColorRed), typeof(ColorBlue));

            var colorsObject = container.Resolve(typeof(IColor[]));
            var colors = (object[])colorsObject;
            var actualCount = colors.Length;

            Assert.AreEqual(EXPECTED_COUNT, actualCount);
        }

        [Test]
        public void Resolve_Singleton_Contract()
        {
            var container = Container.Create();

            container.Register<ITimeMachine, TimeMachineDelorean>(Scope.Singleton);

            var machineFirst = container.Resolve<ITimeMachine>();
            var machineSecond = container.Resolve<ITimeMachine>();

            Assert.AreSame(machineFirst, machineSecond);
        }

        [Test]
        public void Resolve_Singleton_Result()
        {
            var container = Container.Create();

            container.Register<TimeMachineDelorean>(Scope.Singleton);

            var machineFirst = container.Resolve<TimeMachineDelorean>();
            var machineSecond = container.Resolve<TimeMachineDelorean>();

            Assert.AreSame(machineFirst, machineSecond);
        }

        [Test]
        public void Resolve_Singleton_Result_Throws_TypeNotRegisteredException()
        {
            var container = Container.Create();

            container.Register<ITimeMachine, TimeMachineDelorean>(Scope.Singleton);

            Assert.Throws(typeof(TypeNotRegisteredException), () =>
            {
                container.Resolve<TimeMachineDelorean>();
            });
        }

        [Test]
        public void Resolve_Factory()
        {
            var container = Container.Create();

            container.RegisterFactory<CarFactory>();

            var factory = container.Resolve<CarFactory>();
            var result = factory.Create<CarTruck>();

            Assert.IsInstanceOf<ICar>(result);
        }

        [Test]
        public void Resolve_Installer_FromString()
        {
            var container = Container.Create();

            var installerName = typeof(InstallerDummy).FullName;
            var assembly = typeof(InstallerDummy).Assembly;

            container.Install(assembly, installerName);
        }
    }
}