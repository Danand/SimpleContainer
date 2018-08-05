﻿using System;
using System.Collections.Generic;

using SimpleContainer.Factories;
using SimpleContainer.Interfaces;

namespace SimpleContainer
{
    public sealed partial class SimpleContainer
    {
        private readonly Dispatcher dispatcher = new Dispatcher();
        private readonly Dictionary<Type, Resolver> bindings = new Dictionary<Type, Resolver>();

        public static SimpleContainer Create()
        {
            return new SimpleContainer();
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

        public void RegisterFactory<TFactory>()
            where TFactory : IFactory
        {
            Register<TFactory>(Scope.Singleton);
            var factory = Resolve<TFactory>();
            factory.Container = this;
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

        public bool CheckRegistered<TContract>()
        {
            return CheckRegistered(typeof(TContract));
        }

        public bool CheckRegistered(Type contractType)
        {
            return bindings.ContainsKey(contractType);
        }

        public TResult Resolve<TResult>()
        {
            return (TResult)Resolve(typeof(TResult));
        }

        public object Resolve(Type contractType)
        {
            return Resolve(contractType, new object[0]);
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

        private void Initialize(object result)
        {
            if (result is IInitializible initializible)
                initializible.Initialize();
        }
    }
}