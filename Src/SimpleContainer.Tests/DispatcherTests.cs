﻿using SimpleContainer.Tests.DummyTypes;

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

            container.Register<ICustomHandler, CustomHandler>(Scope.Singleton);
            container.Register<DummyInvoker>(Scope.Singleton);
            container.RegisterEvent<ICustomHandler, CustomArgs>((handler, args) => handler.OnCustomEvent(args));

            var invoker = container.Resolve<DummyInvoker>();
            var eventHandler = container.Resolve<ICustomHandler>();

            var expectedValue = new CustomArgs
            {
                flag = true,
                id = 9,
                name = "shine"
            };

            invoker.RaiseEvent(expectedValue);

            var actualValue = eventHandler.ReceivedArgs;

            Assert.AreEqual(expectedValue, actualValue);
        }

        [Test]
        public void Dispatcher_Send_Singleton_Shortcut()
        {
            var container = Container.Create();

            container.Register<ICustomHandler, CustomHandler>(Scope.Singleton);
            container.Register<DummyInvoker>(Scope.Singleton);
            container.RegisterEvent<ICustomHandler, CustomArgs>();

            var invoker = container.Resolve<DummyInvoker>();
            var eventHandler = container.Resolve<ICustomHandler>();

            var expectedValue = new CustomArgs
            {
                flag = true,
                id = 9,
                name = "shine"
            };

            invoker.RaiseEvent(expectedValue);

            var actualValue = eventHandler.ReceivedArgs;

            Assert.AreEqual(expectedValue, actualValue);
        }

        [Test]
        public void Dispatcher_Send_Factory()
        {
            var container = Container.Create();

            container.Register<DummyInvoker>(Scope.Singleton);
            container.RegisterFactory<CustomHandlerFactory>();
            container.RegisterEvent<ICustomHandler, CustomArgs>((handler, args) => handler.OnCustomEvent(args));

            var invoker = container.Resolve<DummyInvoker>();
            var factory = container.Resolve<CustomHandlerFactory>();

            var eventHandler = factory.Create();

            var expectedValue = new CustomArgs
            {
                flag = true,
                id = 9,
                name = "shine"
            };

            invoker.RaiseEvent(expectedValue);

            var actualValue = eventHandler.ReceivedArgs;

            Assert.AreEqual(expectedValue, actualValue);
        }

        [Test]
        public void Dispatcher_Send_All()
        {
            var container = Container.Create();

            container.Register<DummyInvoker>(Scope.Singleton);
            container.Register<CustomHandler>(Scope.Singleton);
            container.RegisterEvent<ICustomHandler, CustomArgs>((handler, args) => handler.OnCustomEvent(args));

            var invoker = container.Resolve<DummyInvoker>();
            var eventHandler = container.Resolve<CustomHandler>();

            var expectedValue = new CustomArgs
            {
                flag = true,
                id = 9,
                name = "shine"
            };

            invoker.RaiseEvent(expectedValue);

            var actualValue = eventHandler.ReceivedArgs;

            Assert.AreEqual(expectedValue, actualValue);
        }
    }
}