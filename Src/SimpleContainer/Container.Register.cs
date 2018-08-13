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

        public void Register(Type resultType, Type contractType)
        {
            Register(resultType, contractType, Scope.Transient, null);
        }

        public void Register(Type resultType, Type contractType, Scope scope)
        {
            Register(resultType, contractType, scope, null);
        }

        public void RegisterFactory<TFactory>()
            where TFactory : IFactory
        {
            Register<TFactory>(Scope.Singleton);
            var factory = Resolve<TFactory>();
            factory.Container = this;
        }

        public bool CheckRegistered<TContract>()
        {
            return CheckRegistered(typeof(TContract));
        }
    }
}