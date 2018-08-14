using System;
using System.Collections.Generic;

using SimpleContainer.Factories;
using SimpleContainer.Interfaces;

namespace SimpleContainer
{
    public sealed partial class Container
    {
        private readonly Dispatcher dispatcher = new Dispatcher();
        private readonly Dictionary<Type, Resolver> bindings = new Dictionary<Type, Resolver>();

        public static Container Create()
        {
            return new Container();
        }

        public void Register(
            Type            contractType,
            Type            resultType,
            Scope           scope,
            object          instance,
            params object[] args)
        {
            if (!bindings.ContainsKey(contractType))
                bindings.Add(contractType, new Resolver(this, resultType, scope, instance, args));
        }

        public void RegisterFactory<TFactoryContract, TFactoryResult>()
            where TFactoryContract : IFactory
            where TFactoryResult : TFactoryContract
        {
            Register<TFactoryContract, TFactoryResult>(Scope.Singleton);
            var factory = Resolve<TFactoryContract>();
            factory.Container = this;
        }

        public void RegisterEvent<TEventHandler, TEventArgs>(Action<TEventHandler, TEventArgs> action)
        {
            dispatcher.RegisterEvent(this, action);
        }

        public void SendEvent<TEventArgs>(TEventArgs eventArgs)
        {
            dispatcher.Send(eventArgs);
        }

        public bool CheckRegistered(Type contractType)
        {
            return bindings.ContainsKey(contractType);
        }

        public object Resolve(Type contractType, params object[] args)
        {
            if (!bindings.TryGetValue(contractType, out var resolver))
                throw new TypeNotRegisteredException(contractType);

            var result = resolver.GetInstance(args);

            Initialize(result);

            return result;
        }

        public void Dispose()
        {
            foreach (var resolver in bindings.Values)
                resolver.DisposeInstances();
        }

        public void Install(params IInstaller[] installers)
        {
            foreach (var installer in installers)
                installer.Install(this);
        }

        internal object[] GetAllCached<TContract>()
        {
            var contractType = typeof(TContract);

            if (!bindings.TryGetValue(contractType, out var resolver))
                throw new TypeNotRegisteredException(contractType);

            return resolver.GetCachedInstances();
        }

        private void Initialize(object result)
        {
            if (result is IInitializible initializible)
                initializible.Initialize();
        }
    }
}