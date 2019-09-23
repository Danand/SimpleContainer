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
        private readonly HashSet<InstanceWrapper> transientInstances = new HashSet<InstanceWrapper>();
        private readonly HashSet<MemberInfo> injectedIntoMembers = new HashSet<MemberInfo>();

        private List<InstanceWrapper> singleInstances = new List<InstanceWrapper>();

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

            prePassedArgs = args;

            if (instance != null)
            {
                switch (scope)
                {
                    case Scope.Transient:
                        transientInstances = new HashSet<InstanceWrapper> { new InstanceWrapper(instance) };
                        break;

                    case Scope.Singleton:
                        singleInstances = new List<InstanceWrapper> { new InstanceWrapper(instance) };
                        break;
                }
            }
        }

        internal Type[] ResultTypes
        {
            get { return resultTypes; }
        }

        public InstanceWrapper[] GetInstances(object[] args)
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
                    if (singleInstances.Count > 0)
                    {
                        foreach (var instance in singleInstances)
                            InjectIntoInstance(instance.Value);

                        return singleInstances.ToArray();
                    }

                    var singleInstancesArray = CreateInstances(resultTypes, resultArgs);

                    singleInstances = singleInstancesArray.ToList();

                    return singleInstancesArray;

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
                        DisposeInstance(transientInstance, transientInstances);
                    break;

                case Scope.Singleton:
                    foreach (var singleInstance in singleInstances)
                        DisposeInstance(singleInstance, singleInstances);
                    break;

                default:
                    throw new ArgumentException(nameof(scope));
            }
        }

        internal IEnumerable<InstanceWrapper> GetCachedInstances()
        {
            switch (scope)
            {
                case Scope.Transient:
                    return transientInstances;

                case Scope.Singleton:
                    return singleInstances;

                default:
                    throw new ArgumentException(nameof(scope));
            }
        }

        private ConstructorInfo GetConstructor(Type type)
        {
            if (container.cachedConstructors.TryGetValue(type, out var suitableConstructor))
                return suitableConstructor;

            var constructors = type.GetConstructors();
            suitableConstructor = constructors[0];

            container.cachedConstructors.Add(type, suitableConstructor);

            return suitableConstructor;
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

        private void DisposeInstance(InstanceWrapper instance, ICollection<InstanceWrapper> instances)
        {
            if (instance.Value is IDisposable disposable)
            {
                instances.Remove(instance);
                disposable.Dispose();
            }
        }

        private InstanceWrapper[] CreateInstances(Type[] types, object[] args)
        {
            var typesLength = types.Length;
            var result = new InstanceWrapper[typesLength];

            for (var i = 0; i < typesLength; i++)
            {
                var type = types[i];
                var suitableConstructor = GetConstructor(type);
                var resolvedArgs = ResolveArgs(suitableConstructor, args);
                var instance = activator.CreateInstance(suitableConstructor, resolvedArgs);

                InjectIntoInstance(instance);

                result[i] = new InstanceWrapper(instance);
            }

            return result;
        }

        private bool CheckNeedsInjectIntoMember(MemberInfo member)
        {
            return !injectedIntoMembers.Contains(member);
        }

        private void MarkMemberInjectedInto(MemberInfo member)
        {
            injectedIntoMembers.Add(member);
        }

        private void InjectIntoInstance(object instance)
        {
            ResolveFields(instance);
            ResolveProperties(instance);
            ResolveMethods(instance);
        }

        private void ResolveFields(object instance)
        {
            var type = instance.GetType();
            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            var injectableFields = fields.Where(field => field.GetCustomAttributes(container.injectAttributeType).Any()).ToArray();

            foreach (var field in injectableFields)
            {
                if (CheckNeedsInjectIntoMember(field))
                {
                    var values = container.ResolveMultiple(field.FieldType);
                    var collected = CollectValue(field.FieldType, values);

                    field.SetValue(instance, collected.Value);

                    MarkMemberInjectedInto(field);
                }
            }
        }

        private void ResolveProperties(object instance)
        {
            var type = instance.GetType();
            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            var injectableProperties = properties.Where(field => field.GetCustomAttributes(container.injectAttributeType).Any()).ToArray();

            foreach (var property in injectableProperties)
            {
                if (CheckNeedsInjectIntoMember(property))
                {
                    var values = container.ResolveMultiple(property.PropertyType);
                    var collected = CollectValue(property.PropertyType, values);

                    property.SetValue(instance, collected.Value);

                    MarkMemberInjectedInto(property);
                }
            }
        }

        private void ResolveMethods(object instance)
        {
            var type = instance.GetType();
            var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            var injectableMethods = methods.Where(method => method.GetCustomAttributes(container.injectAttributeType).Any()).ToArray();

            foreach (var method in injectableMethods)
            {
                if (CheckNeedsInjectIntoMember(method))
                {
                    var args = new List<object>();
                    var parameters = method.GetParameters();

                    foreach (var parameter in parameters)
                    {
                        var values = container.ResolveMultiple(parameter.ParameterType);
                        var collected = CollectValue(parameter.ParameterType, values);

                        args.Add(collected.Value);
                    }

                    method.Invoke(instance, args.ToArray());

                    MarkMemberInjectedInto(method);
                }
            }
        }

        private CollectedValue CollectValue(Type returnType, object[] values)
        {
            return new CollectedValue(returnType, values);
        }
    }
}