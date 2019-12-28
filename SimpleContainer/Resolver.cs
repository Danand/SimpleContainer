using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using SimpleContainer.Interfaces;

namespace SimpleContainer
{
    internal sealed class Resolver
    {
        private readonly IActivator activator;
        private readonly IConstructorCacher constructorCacher;
        private readonly ArrayArgumentConverter argConverter = new ArrayArgumentConverter();
        private readonly HashSet<int> injectedIntoMembers = new HashSet<int>();
        private readonly HashSet<object> injectedIntoInstances = new HashSet<object>();

        private Container container;
        private Scope scope;
        private IList<object> prePassedArgs;
        private Func<Resolver> method;

        public Resolver(IActivator activator, IConstructorCacher constructorCacher)
        {
            this.activator = activator;
            this.constructorCacher = constructorCacher;
        }

        internal IList<Type> ResultTypes { get; private set; }

        internal HashSet<object> Instances { get; private set; } = new HashSet<object>();

        public void Initialize(
            Container       container,
            IList<Type>     resultTypes,
            Scope           scope,
            IList<object>   instances,
            IList<object>   args)
        {
            this.container = container;
            this.ResultTypes = resultTypes;
            this.scope = scope;

            prePassedArgs = args;

            if (instances != null)
            {
                this.Instances = new HashSet<object>(instances.Select(instance => instance));
            }
        }

        public ICollection<object> GetInstances(object[] args)
        {
            var resultArgs = prePassedArgs.Count > args.Length ? prePassedArgs : args;

            switch (scope)
            {
                case Scope.Transient:
                    var newInstances = CreateInstances(ResultTypes, resultArgs);

                    foreach (var newInstance in newInstances)
                        Instances.Add(newInstance);

                    return newInstances;

                case Scope.Singleton:
                    for (var i = 0; i < ResultTypes.Count; i++)
                    {
                        var resultType = ResultTypes[i];

                        if (i >= Instances.Count)
                            Instances.Add(CreateInstanceFromActivator(resultType, args));
                    }

                    return Instances;

                default:
                    throw new ArgumentException(nameof(scope));
            }
        }

        public void DisposeInstances()
        {
            foreach (var transientInstance in Instances.ToArray())
                DisposeInstance(transientInstance, Instances);
        }

        public Resolver CopyToContainer(Container other)
        {
            var resolver = new Resolver(activator, constructorCacher);

            resolver.Initialize(
                container:      other,
                resultTypes:    ResultTypes,
                scope:          scope,
                instances:      Instances.Select(instance => instance).ToArray(),
                args:           prePassedArgs);

            return resolver;
        }

        internal IEnumerable<object> GetCachedInstances()
        {
            return Instances;
        }

        internal void InjectIntoInstance(object instance)
        {
            if (injectedIntoInstances.Add(instance))
            {
                ResolveFields(instance);
                ResolveProperties(instance);
                ResolveMethods(instance);
            }
        }

        internal void SetMethod(Func<Resolver> method)
        {
            this.method = method;
        }

        internal void RemoveMethod()
        {
            method = null;
        }

        private object[] ResolveArgs(ConstructorInfo constructorInfo, IList<object> args)
        {
            var parameters = constructorInfo.GetParameters();

            if (parameters.Length == 0)
                return new object[0];

            var result = new object[parameters.Length];

            for (var i = 0; i < parameters.Length; i++)
            {
                var parameterInfo = parameters[i];
                var parameterType = parameterInfo.ParameterType;
                var lessArgs = i >= args.Count;

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

        private void DisposeInstance(object instance, ICollection<object> instances)
        {
            if (instance is IDisposable disposable)
            {
                instances.Remove(instance);
                disposable.Dispose();
            }
        }

        private object[] CreateInstances(IList<Type> types, IList<object> args)
        {
            var typesLength = types.Count;
            var result = new object[typesLength];

            for (var i = 0; i < typesLength; i++)
            {
                object instance;

                if (method == null)
                    instance = CreateInstanceFromActivator(types[i], args);
                else
                    instance = CreateInstanceFromMethod();

                result[i] = instance;
            }

            return result;
        }

        private object CreateInstanceFromActivator(Type type, IList<object> args)
        {
            var suitableConstructor = constructorCacher.GetConstructor(type);
            var resolvedArgs = ResolveArgs(suitableConstructor, args);
            var instance = activator.CreateInstance(suitableConstructor, resolvedArgs);

            InjectIntoInstance(instance);

            return instance;
        }

        private object CreateInstanceFromMethod()
        {
            var instance = method.Invoke();
            InjectIntoInstance(instance);
            return instance;
        }

        private bool CheckNeedsInjectIntoMember(MemberInfo member, object instance)
        {
            return !injectedIntoMembers.Contains(member.GetHashCode() + instance.GetHashCode());
        }

        private void MarkMemberInjectedInto(MemberInfo member, object instance)
        {
            injectedIntoMembers.Add(member.GetHashCode() + instance.GetHashCode());
        }

        private void ResolveFields(object instance)
        {
            var type = instance.GetType();
            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            var injectableFields = fields.Where(field => container.injectAttributeTypes.Any(injectAttributeType => field.GetCustomAttributes(injectAttributeType).Any()));

            foreach (var field in injectableFields)
            {
                if (CheckNeedsInjectIntoMember(field, instance))
                {
                    var values = container.ResolveMultiple(field.FieldType);
                    var collected = CollectValue(field.FieldType, values);

                    field.SetValue(instance, collected.Value);

                    MarkMemberInjectedInto(field, instance);
                }
            }
        }

        private void ResolveProperties(object instance)
        {
            var type = instance.GetType();
            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            var injectableProperties = properties.Where(property => container.injectAttributeTypes.Any(injectAttributeType => property.GetCustomAttributes(injectAttributeType).Any()));

            foreach (var property in injectableProperties)
            {
                if (CheckNeedsInjectIntoMember(property, instance))
                {
                    var values = container.ResolveMultiple(property.PropertyType);
                    var collected = CollectValue(property.PropertyType, values);

                    property.SetValue(instance, collected.Value);

                    MarkMemberInjectedInto(property, instance);
                }
            }
        }

        private void ResolveMethods(object instance)
        {
            var type = instance.GetType();
            var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            var injectableMethods = methods.Where(method => container.injectAttributeTypes.Any(injectAttributeType => method.GetCustomAttributes(injectAttributeType).Any()));

            foreach (var method in injectableMethods)
            {
                if (CheckNeedsInjectIntoMember(method, instance))
                {
                    var parameters = method.GetParameters();
                    var args = new object[parameters.Length];

                    for (var i = 0; i < parameters.Length; i++)
                    {
                        var parameter = parameters[i];
                        var values = container.ResolveMultiple(parameter.ParameterType);
                        var collected = CollectValue(parameter.ParameterType, values);

                        args[i] = collected.Value;
                    }

                    method.Invoke(instance, args);

                    MarkMemberInjectedInto(method, instance);
                }
            }
        }

        private CollectedValue CollectValue(Type returnType, object[] values)
        {
            return new CollectedValue(returnType, values);
        }
    }
}