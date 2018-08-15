using System;

using SimpleContainer.Factories;

namespace SimpleContainer
{
    public sealed partial class Container
    {
        public void Register<TResult>()
        {
            Register(typeof(TResult), typeof(TResult), Scope.Transient, null);
        }

        public void Register<TResult>(Scope scope)
        {
            Register(typeof(TResult), typeof(TResult), scope, null);
        }

        public void Register<TResult>(Scope scope, TResult instance)
        {
            Register(typeof(TResult), typeof(TResult), scope, instance);
        }

        public void Register<TContract>(params Type[] resultTypes)
        {
            Register(typeof(TContract), Scope.Transient, resultTypes);
        }

        public void Register<TContract, TResult>()
            where TResult : TContract
        {
            Register(typeof(TContract), typeof(TResult), Scope.Transient, null);
        }

        public void Register<TContract, TResult>(Scope scope)
            where TResult : TContract
        {
            Register(typeof(TContract), typeof(TResult), scope, null);
        }

        public void Register<TContract, TResult>(Scope scope, TResult instance)
            where TResult : TContract
        {
            Register(typeof(TContract), typeof(TResult), scope, instance);
        }

        public void Register<TContract, TResult>(Scope scope, params object[] args)
            where TResult : TContract
        {
            Register(typeof(TContract), typeof(TResult), scope, null, args);
        }

        public void Register(Type resultType)
        {
            Register(resultType, resultType, Scope.Transient, null);
        }

        public void Register(Type contractType, Type resultType)
        {
            Register(contractType, resultType, Scope.Transient, null);
        }

        public void Register(Type contractType, Scope scope, params Type[] resultTypes)
        {
            RegisterInner(contractType, scope, null, resultTypes, new object[0]);
        }

        public void Register(Type contractType, Type resultType, Scope scope)
        {
            Register(contractType, resultType, scope, null);
        }

        public void Register(
            Type            contractType,
            Type            resultType,
            Scope           scope,
            object          instance,
            params object[] args)
        {
            RegisterInner(contractType, scope, instance, new [] { resultType }, args);
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

        public bool CheckRegistered<TContract>()
        {
            return CheckRegistered(typeof(TContract));
        }

        public bool CheckRegistered(Type contractType)
        {
            return bindings.ContainsKey(contractType);
        }

        private void RegisterInner(
            Type        contractType,
            Scope       scope,
            object      instance,
            Type[]      resultTypes,
            object[]    args)
        {
            if (!bindings.ContainsKey(contractType))
                bindings.Add(contractType, new Resolver(this, resultTypes, scope, instance, args));
        }
    }
}