using System;
using System.Collections.Generic;
using System.Reflection;

namespace SimpleContainer
{
    internal sealed class Resolver
    {
        private readonly SimpleContainer container;
        private readonly Type resultType;
        private readonly Scope scope;
        private readonly object[] prePassedArgs;
        private readonly HashSet<object> transientInstances = new HashSet<object>();

        private object singleInstance;

        public Resolver(
            SimpleContainer container,
            Type            resultType,
            Scope           scope,
            object          instance,
            params object[] args)
        {
            this.container = container;
            this.resultType = resultType;
            this.scope = scope;
            singleInstance = instance;
            prePassedArgs = args;
        }

        public object GetInstance(object[] args)
        {
            var resultArgs = prePassedArgs.Length > args.Length ? prePassedArgs : args;

            switch (scope)
            {
                case Scope.Transient:
                    var newInstance = CreateInstance(resultType, resultArgs);
                    transientInstances.Add(newInstance);
                    return newInstance;

                case Scope.Singleton:
                    return singleInstance = (singleInstance ?? CreateInstance(resultType, resultArgs));

                default:
                    throw new ArgumentException(nameof(scope));
            }
        }

        public void DisposeInstances()
        {
            switch (scope)
            {
                case Scope.Transient:
                    foreach (var transientInstance in transientInstances)
                        DisposeInstance(transientInstance);
                    break;

                case Scope.Singleton:
                    DisposeInstance(singleInstance);
                    break;

                default:
                    throw new ArgumentException(nameof(scope));
            }
        }

        private object[] ResolveArgs(ConstructorInfo constructorInfo, object[] args)
        {
            var parameters = constructorInfo.GetParameters();

            if (parameters.Length == 0)
                return new object[0];

            var result = new object[parameters.Length];

            for (var i = 0; i < parameters.Length; i++)
            {
                var parameterInfo = parameters[i];
                var lessArgs = i >= args.Length;

                if (lessArgs)
                {
                    result[i] = container.Resolve(parameterInfo.ParameterType);
                }
                else
                {
                    var arg = args[i];

                    if (CheckAssignable(parameterInfo.ParameterType, arg.GetType()))
                        result[i] = arg;
                    else
                        result[i] = container.Resolve(parameterInfo.ParameterType);
                }
            }

            return result;
        }

        private bool CheckAssignable(Type parentType, Type childType)
        {
            return parentType.IsAssignableFrom(childType);
        }

        private void DisposeInstance(object instance)
        {
            if (instance is IDisposable disposable)
                disposable.Dispose();
        }

        private object CreateInstance(Type type, object[] args)
        {
            var constructors = type.GetConstructors();
            var suitableConstructor = constructors[0];

            var resolvedArgs = ResolveArgs(suitableConstructor, args);

            return suitableConstructor.Invoke(resolvedArgs);
        }
    }
}