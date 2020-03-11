using System;
using System.Collections.Generic;
using System.Linq;

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
            var contractIsArray = contractType.IsArray;
            var elementType = contractIsArray ? contractType.GetElementType() : contractType;

            var instances = ResolveAll(elementType);

            if (contractIsArray)
                return instances;

            return instances[0];
        }

        public object[] ResolveAll(Type contractType)
        {
            var instances = resolver.GetInstances(args);

            return instances.Select(instance => instance).ToArray();
        }

        public TContract GetCached<TContract>()
        {
            var contractType = typeof(TContract);
            return (TContract)resolver.GetCachedInstances().First();
        }

        public TContract[] GetCachedMultiple<TContract>()
        {
            var contractType = typeof(TContract);
            return resolver.GetCachedInstances().Select(instance => (TContract)instance).ToArray();
        }

        public void InjectInto(object instance)
        {
            foreach (var binding in bindings.Values)
                binding.InjectIntoInstance(instance);
        }

        public IEnumerable<object> GetAllCachedInstances()
        {
            return GetAllCached().Select(wrapper => wrapper);
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
            IActivator activator = Resolve<IActivator>();
            return (IInstaller)activator.CreateInstance(installerType);
        }
    }
}