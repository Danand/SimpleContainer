using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using SimpleContainer.Interfaces;

namespace SimpleContainer
{
    internal sealed class Resolver
    {
        private readonly Container container;
        private readonly IActivator activator;
        private readonly Type[] resultTypes;
        private readonly Scope scope;
        private readonly object[] prePassedArgs;
        private readonly ArrayArgumentConverter argConverter = new ArrayArgumentConverter();
        private readonly HashSet<object> transientInstances = new HashSet<object>();

        private object[] singleInstances;

        public Resolver(
            Container       container,
            IActivator      activator,
            Type[]          resultTypes,
            Scope           scope,
            object          instance,
            params object[] args)
        {
            this.container = container;
            this.activator = activator;
            this.resultTypes = resultTypes;
            this.scope = scope;

            if (instance != null)
                singleInstances = new [] { instance };

            prePassedArgs = args;
        }

        public object[] GetInstances(object[] args)
        {
            var resultArgs = prePassedArgs.Length > args.Length ? prePassedArgs : args;

            switch (scope)
            {
                case Scope.Transient:
                    var newInstances = CreateInstances(resultTypes, resultArgs);

                    foreach (var newInstance in newInstances)
                        transientInstances.Add(newInstance);

                    return newInstances;

                case Scope.Singleton:
                    return singleInstances = (singleInstances ?? CreateInstances(resultTypes, resultArgs));

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
                    foreach (var singleInstance in singleInstances)
                        DisposeInstance(singleInstance);
                    break;

                default:
                    throw new ArgumentException(nameof(scope));
            }
        }

        internal object[] GetCachedInstances()
        {
            switch (scope)
            {
                case Scope.Transient:
                    return transientInstances.ToArray();

                case Scope.Singleton:
                    return singleInstances;

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
                var parameterType = parameterInfo.ParameterType;
                var lessArgs = i >= args.Length;

                if (lessArgs)
                {
                    result[i] = container.Resolve(parameterType);
                }
                else
                {
                    var arg = args[i];

                    if (CheckAssignable(parameterType, arg.GetType()))
                        result[i] = arg;
                    else
                        result[i] = container.Resolve(parameterType);
                }

                result = argConverter.Convert(parameterType, result);
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

        private object[] CreateInstances(Type[] types, object[] args)
        {
            var typesLength = types.Length;
            var result = new object[typesLength];

            for (var i = 0; i < typesLength; i++)
            {
                var type = types[i];
                var constructors = type.GetConstructors();
                var suitableConstructor = constructors[0];
                var resolvedArgs = ResolveArgs(suitableConstructor, args);
                var instance = activator.CreateInstance(suitableConstructor, resolvedArgs);

                result[i] = instance;
            }

            return result;
        }
    }
}