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
        private Type[] resultTypes;
        private object[] prePassedArgs;
        private HashSet<InstanceWrapper> instances = new HashSet<InstanceWrapper>();
        private Func<Resolver> method;

        public Resolver(IActivator activator, IConstructorCacher constructorCacher)
        {
            this.activator = activator;
            this.constructorCacher = constructorCacher;
        }

        internal Type[] ResultTypes
        {
            get { return resultTypes; }
        }

        internal HashSet<InstanceWrapper> Instances
        {
            get { return instances; }
        }

        public void Initialize(
            Container   container,
            Type[]      resultTypes,
            Scope       scope,
            object[]    instances,
            object[]    args)
        {
            this.container = container;
            this.resultTypes = resultTypes;
            this.scope = scope;

            prePassedArgs = args;

            if (instances != null)
            {
                this.instances = new HashSet<InstanceWrapper>(instances.Select(instance => new InstanceWrapper(instance)));
            }
        }

        public ICollection<InstanceWrapper> GetInstances(object[] args)
        {
            var resultArgs = prePassedArgs.Length > args.Length ? prePassedArgs : args;

            switch (scope)
            {
                case Scope.Transient:
                    var newInstances = CreateInstances(resultTypes, resultArgs);

                    foreach (var newInstance in newInstances)
                        instances.Add(newInstance);

                    return newInstances;

                case Scope.Singleton:
                    for (var i = 0; i < resultTypes.Length; i++)
                    {
                        var resultType = resultTypes[i];

                        if (i >= instances.Count)
                            instances.Add(CreateInstanceFromActivator(resultType, args));
                    }

                    return instances;

                default:
                    throw new ArgumentException(nameof(scope));
            }
        }

        public void DisposeInstances()
        {
            foreach (var transientInstance in instances.ToArray())
                DisposeInstance(transientInstance, instances);
        }

        public Resolver CopyToContainer(Container other)
        {
            var resolver = new Resolver(activator, constructorCacher);

            resolver.Initialize(
                container:      other,
                resultTypes:    resultTypes,
                scope:          scope,
                instances:      instances.Select(instance => instance.Value).ToArray(),
                args:           prePassedArgs);

            return resolver;
        }

        internal IEnumerable<InstanceWrapper> GetCachedInstances()
        {
            return instances;
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
                InstanceWrapper wrapper;

                if (method == null)
                    wrapper = CreateInstanceFromActivator(types[i], args);
                else
                    wrapper = CreateInstanceFromMethod();

                result[i] = wrapper;
            }

            return result;
        }

        private InstanceWrapper CreateInstanceFromActivator(Type type, object[] args)
        {
            var suitableConstructor = constructorCacher.GetConstructor(type);
            var resolvedArgs = ResolveArgs(suitableConstructor, args);
            var instance = activator.CreateInstance(suitableConstructor, resolvedArgs);

            InjectIntoInstance(instance);

            return new InstanceWrapper(instance);
        }

        private InstanceWrapper CreateInstanceFromMethod()
        {
            var instance = method.Invoke();
            InjectIntoInstance(instance);
            return new InstanceWrapper(instance);
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
            var injectableFields = fields.Where(field => field.GetCustomAttributes(container.injectAttributeType).Any()).ToArray();

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
            var injectableProperties = properties.Where(property => property.GetCustomAttributes(container.injectAttributeType).Any()).ToArray();

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
            var injectableMethods = methods.Where(method => method.GetCustomAttributes(container.injectAttributeType).Any()).ToArray();

            foreach (var method in injectableMethods)
            {
                if (CheckNeedsInjectIntoMember(method, instance))
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