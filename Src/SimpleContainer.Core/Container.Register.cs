using System;
using System.Collections.Generic;
using System.Linq;

using SimpleContainer.Activators;
using SimpleContainer.Exceptions;
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
            if (instance == null)
                throw new NullInstanceException(typeof(TResult));

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
            if (instance == null)
                throw new NullInstanceException(typeof(TResult));

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

        public void RegisterAttribute<TInjectAttribute>()
            where TInjectAttribute : Attribute
        {
            injectAttributeType = typeof(TInjectAttribute);
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
            if (bindings.TryGetValue(contractType, out var foundBinding))
            {
                var resolver = MergeResolver(contractType, scope, instance, resultTypes, args, foundBinding);
                bindings.Remove(contractType);
                bindings.Add(contractType, resolver);
            }
            else
            {
                var resolver = CreateResolver(scope, instance, resultTypes, args);
                bindings.Add(contractType, resolver);
            }
        }

        private Resolver CreateResolver(Scope scope, object instance, Type[] resultTypes, object[] args)
        {
            var resolver = new Resolver(
                container:      this,
                activator:      new ActivatorExpression(),
                resultTypes:    resultTypes,
                scope:          scope,
                instances:      instance == null ? null : new[] {instance},
                args:           args);

            return resolver;
        }

        private Resolver MergeResolver(Type contractType, Scope scope, object instance, Type[] resultTypes, object[] args, Resolver foundBinding)
        {
            var mergedResultTypes = new List<Type>();

            mergedResultTypes.AddRange(foundBinding.ResultTypes);
            mergedResultTypes.AddRange(resultTypes);

            var mergedInstances = new List<object>();

            mergedInstances.AddRange(foundBinding.Instances.Select(wrapper => wrapper.Value));

            if (instance != null)
                mergedInstances.Add(instance);

            var resolver = new Resolver(
                container:      this,
                activator:      new ActivatorExpression(),
                resultTypes:    mergedResultTypes.ToArray(),
                scope:          scope,
                instances:      mergedInstances.ToArray(),
                args:           args);

            return resolver;
        }
    }
}