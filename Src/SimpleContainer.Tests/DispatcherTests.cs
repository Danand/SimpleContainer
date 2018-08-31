using SimpleContainer.Tests.DummyTypes;

using NUnit.Framework;

namespace SimpleContainer.Tests
{
    [TestFixture]
    public class DispatcherTests
    {
        [Test]
        public void Dispatcher_Send_Singleton_Exact()
        {
            var container = Container.Create();

            container.Register<ICustomEventHandler, CustomEventHandler>(Scope.Singleton);
            container.Register<DummyInvoker>(Scope.Singleton);
            container.RegisterEvent<ICustomEventHandler, CustomEventArgs>((handler, args) => handler.OnCustomEvent(args));

            var invoker = container.Resolve<DummyInvoker>();
            var eventHandler = container.Resolve<ICustomEventHandler>();

            var expectedValue = new CustomEventArgs
            {
                flag = true,
                id = 9,
                name = "shine"
            };

            invoker.RaiseEvent(expectedValue);

            var actualValue = eventHandler.ReceivedEventArgs;

            Assert.AreEqual(expectedValue, actualValue);
        }

        [Test]
        public void Dispatcher_Send_Singleton_Shortcut()
        {
            var container = Container.Create();

            container.Register<ICustomEventHandler, CustomEventHandler>(Scope.Singleton);
            container.Register<DummyInvoker>(Scope.Singleton);
            container.RegisterEvent<ICustomEventHandler, CustomEventArgs>();

            var invoker = container.Resolve<DummyInvoker>();
            var eventHandler = container.Resolve<ICustomEventHandler>();

            var expectedValue = new CustomEventArgs
            {
                flag = true,
                id = 9,
                name = "shine"
            };

            invoker.RaiseEvent(expectedValue);

            var actualValue = eventHandler.ReceivedEventArgs;

            Assert.AreEqual(expectedValue, actualValue);
        }

        [Test]
        public void Dispatcher_Send_Factory()
        {
            var container = Container.Create();

            container.Register<DummyInvoker>(Scope.Singleton);
            container.RegisterFactory<CustomEventHandlerFactory>();
            container.RegisterEvent<ICustomEventHandler, CustomEventArgs>((handler, args) => handler.OnCustomEvent(args));

            var invoker = container.Resolve<DummyInvoker>();
            var factory = container.Resolve<CustomEventHandlerFactory>();

            var eventHandler = factory.Create();

            var expectedValue = new CustomEventArgs
            {
                flag = true,
                id = 9,
                name = "shine"
            };

            invoker.RaiseEvent(expectedValue);

            var actualValue = eventHandler.ReceivedEventArgs;

            Assert.AreEqual(expectedValue, actualValue);
        }

        [Test]
        public void Dispatcher_Send_All()
        {
            var container = Container.Create();

            container.Register<DummyInvoker>(Scope.Singleton);
            container.Register<CustomEventHandler>(Scope.Singleton);
            container.RegisterEvent<ICustomEventHandler, CustomEventArgs>((handler, args) => handler.OnCustomEvent(args));

            var invoker = container.Resolve<DummyInvoker>();
            var eventHandler = container.Resolve<CustomEventHandler>();

            var expectedValue = new CustomEventArgs
            {
                flag = true,
                id = 9,
                name = "shine"
            };

            invoker.RaiseEvent(expectedValue);

            var actualValue = eventHandler.ReceivedEventArgs;

            Assert.AreEqual(expectedValue, actualValue);
        }
    }
}