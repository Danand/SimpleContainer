using System;

using SimpleContainer.Exceptions;

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

        public void Register<TContract>(params Type[] resultTypes)
        {
            foreach (Type resultType in resultTypes)
            {
                Register(typeof(TContract), resultType, Scope.Transient, null);
            }
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

        public void Register(Scope scope, object instance)
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));

            var resultType = instance.GetType();

            Register(resultType, resultType, scope, instance);
        }

        public void Register<TContract>(Scope scope, object instance)
        {
            if (instance == null)
                throw new NullInstanceException(typeof(TContract));

            Register(typeof(TContract), instance.GetType(), scope, instance);
        }

        public void Register(Type resultType)
        {
            Register(resultType, resultType, Scope.Transient, null);
        }

        public void Register(Type contractType, Type resultType)
        {
            Register(contractType, resultType, Scope.Transient, null);
        }

        public void Register(Type contractType, Type resultType, Scope scope)
        {
            Register(contractType, resultType, scope, null);
        }

        public void Register(string contractTypeName, string resultTypeName, Scope scope)
        {
            var contractType = InternalDependencies.TypeLoader.Load(contractTypeName);
            var resultType = InternalDependencies.TypeLoader.Load(resultTypeName);

            Register(contractType, resultType, scope, null);
        }

        public void Register(
            Type            contractType,
            Type            resultType,
            Scope           scope,
            object          instance)
        {
            DependencyManager.Register(contractType, resultType, scope, instance);
        }

        public void RegisterAttribute<TInjectAttribute>()
            where TInjectAttribute : Attribute
        {
            InjectAttributeTypes.Add(typeof(TInjectAttribute));
        }
    }
}