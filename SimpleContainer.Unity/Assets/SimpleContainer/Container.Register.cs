using System;

using SimpleContainer.Exceptions;
using SimpleContainer.Interfaces;

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
            var typeLoader = Resolve<ITypeLoader>();
            var contractType = typeLoader.Load(contractTypeName);
            var resultType = typeLoader.Load(resultTypeName);

            Register(contractType, resultType, scope, null);
        }

        public void Register(
            Type            contractType,
            Type            resultType,
            Scope           scope,
            object          instance)
        {

        }

        public void RegisterAttribute<TInjectAttribute>()
            where TInjectAttribute : Attribute
        {
            InjectAttributeTypes.Add(typeof(TInjectAttribute));
        }
    }
}