using System;
using System.Linq;

using SimpleContainer.Activators;
using SimpleContainer.Exceptions;
using SimpleContainer.Interfaces;

namespace SimpleContainer
{
    public sealed partial class Container
    {
        public TContract Resolve<TContract>()
        {
            var result = Resolve(typeof(TContract));

            try
            {
                return (TContract)result;
            }
            catch (InvalidCastException exception)
            {
                throw new InvalidCastException($"Cannot cast '{result?.GetType().Name}' to '{typeof(TContract).Name}'!", exception);
            }
        }

        public TContract[] ResolveAll<TContract>()
        {
            return ResolveAll(typeof(TContract)).Cast<TContract>().ToArray();
        }

        public object Resolve(Type contractType)
        {
            return Resolve(contractType, new object[0]);
        }

        public object Resolve(Type contractType, params object[] args)
        {
            var contractIsArray = contractType.IsArray;
            var elementType = contractIsArray ? contractType.GetElementType() : contractType;

            var instances = ResolveAll(elementType, args);

            if (contractIsArray)
                return instances;

            return instances[0];
        }

        public object[] ResolveAll(Type contractType, params object[] args)
        {
            if (!bindings.TryGetValue(contractType, out var resolver))
            {
                throw new TypeNotRegisteredException(contractType, GetBindingsString(bindings));
            }

            var instances = resolver.GetInstances(args);

            Initialize(instances);

            return instances.Select(instance => instance.Value).ToArray();
        }

        public TContract GetCached<TContract>()
        {
            var contractType = typeof(TContract);

            if (!bindings.TryGetValue(contractType, out var resolver))
                throw new TypeNotRegisteredException(contractType, GetBindingsString(bindings));

            return (TContract)resolver.GetCachedInstances().First()?.Value;
        }

        public TContract[] GetCachedMultiple<TContract>()
        {
            var contractType = typeof(TContract);

            if (!bindings.TryGetValue(contractType, out var resolver))
                throw new TypeNotRegisteredException(contractType, GetBindingsString(bindings));

            return resolver.GetCachedInstances().Select(wrapper => (TContract)wrapper.Value).ToArray();
        }

        public void InjectInto(object instance)
        {
            foreach (var binding in bindings.Values)
                binding.InjectIntoInstance(instance);
        }

        internal object[] ResolveMultiple(Type contractType)
        {
            var contractIsArray = contractType.IsArray;
            var elementType = contractIsArray ? contractType.GetElementType() : contractType;
            var instances = ResolveAll(elementType);

            return instances;
        }

        private IInstaller ResolveInstaller(Type installerType)
        {
            IActivator activator = new ActivatorExpression();
            var constructor = installerType.GetConstructor(new Type[0]);

            return (IInstaller)activator.CreateInstance(constructor, new object[0]);
        }
    }
}