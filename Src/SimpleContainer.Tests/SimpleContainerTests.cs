using SimpleContainer.Tests.DummyTypes;

using NUnit.Framework;

using Container = SimpleContainer.SimpleContainer;

namespace SimpleContainer.Tests
{
    [TestFixture]
    public class SimpleContainerTests
    {
        [Test]
        public void Resolve_Transient()
        {
            var container = new Container();

            container.Register<ICar, CarTruck>(Scope.Transient);

            var carFirst = container.Resolve<ICar>();
            var carSecond = container.Resolve<ICar>();

            Assert.AreNotSame(carFirst, carSecond);
        }

        [Test]
        public void Resolve_Singleton_Contract()
        {
            var container = new Container();

            container.Register<ITimeMachine, TimeMachineDelorean>(Scope.Singleton);

            var machineFirst = container.Resolve<ITimeMachine>();
            var machineSecond = container.Resolve<ITimeMachine>();

            Assert.AreSame(machineFirst, machineSecond);
        }

        [Test]
        public void Resolve_Singleton_Result()
        {
            var container = new Container();

            container.Register<TimeMachineDelorean>(Scope.Singleton);

            var machineFirst = container.Resolve<TimeMachineDelorean>();
            var machineSecond = container.Resolve<TimeMachineDelorean>();

            Assert.AreSame(machineFirst, machineSecond);
        }

        [Test]
        public void Resolve_Factory()
        {
            var container = new Container();

            container.RegisterFactory<CarFactory>();

            var factory = container.Resolve<CarFactory>();
            var result = factory.Create();

            Assert.IsInstanceOf<ICar>(result);
        }

        [Test]
        public void Inject_Transient()
        {
            var container = new Container();

            container.Register<IEngine, EngineBig>(Scope.Transient);
            container.Register<ICar, CarFourWheelDrive>(Scope.Transient);

            var carFirst = container.Resolve<ICar>();
            var carSecond = container.Resolve<ICar>();

            Assert.AreNotSame(carFirst.Engine, carSecond.Engine);
        }

        [Test]
        public void Inject_Singleton()
        {
            var container = new Container();

            container.Register<IPhysics, PhysicsPlanetEarth>(Scope.Singleton);
            container.Register<IEngine, EngineMedium>(Scope.Transient);

            var enigneFirst = container.Resolve<IEngine>();
            var enigneSecond = container.Resolve<IEngine>();

            Assert.AreSame(enigneFirst.Physics, enigneSecond.Physics);
        }

        [Test]
        public void Inject_Factory()
        {
            var container = new Container();

            container.Register<IEngine, EngineBig>(Scope.Transient);
            container.Register<ICar, CarFourWheelDrive>(Scope.Transient);

            container.RegisterFactory<CarFactory>();

            var factory = container.Resolve<CarFactory>();
            var car = factory.Create(typeof(CarFourWheelDrive));

            Assert.IsInstanceOf<EngineBig>(car.Engine);
        }

        [Test]
        public void SubscribeTest()
        {
            var container = new Container();

            container.Register<ICustomEventHandler, CustomEventHandler>(Scope.Singleton);
            container.Register<DummyInvoker>(Scope.Singleton);

            container.RegisterEvent<ICustomEventHandler, CustomEventArgs>((handler, args) => handler.OnCustomEvent(args));

            var invoker = container.Resolve<DummyInvoker>();
            var eventHandler = container.Resolve<ICustomEventHandler>();

            invoker.RaiseEvent();

            var result = eventHandler.ReceivedEventArgs;

            Assert.NotNull(result);
        }
    }
}