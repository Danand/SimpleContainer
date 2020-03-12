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

        public object Resolve(Type contractType)
        {
            return DependencyManager.Resolve(contractType);
        }

        public TContract[] ResolveAll<TContract>()
        {
            return ResolveAll(typeof(TContract)).Cast<TContract>().ToArray();
        }

        public object[] ResolveAll(Type contractType)
        {
            return (object[])DependencyManager.Resolve(contractType.MakeArrayType());
        }

        public TContract GetCached<TContract>()
        {
            var contractType = typeof(TContract);
            return (TContract)DependencyManager.GetCachedInstance(contractType);
        }

        public TContract[] GetCachedMultiple<TContract>()
        {
            var contractType = typeof(TContract);
            return DependencyManager.GetCachedInstances(contractType).Cast<TContract>().ToArray();
        }

        public void InjectInto(object instance)
        {
            DependencyManager.InjectIntoInstance(instance);
        }

        public IEnumerable<object> GetAllCachedInstances()
        {
            return GetAllCached().Select(instance => instance);
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