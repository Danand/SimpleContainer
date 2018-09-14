using SimpleContainer.Tests.DummyTypes;

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
    }
}