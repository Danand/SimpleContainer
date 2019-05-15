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
            return (TContract)Resolve(typeof(TContract));
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
                throw new TypeNotRegisteredException(contractType);

            var instances = resolver.GetInstances(args);

            Initialize(instances);

            return instances.Select(instance => instance.Value).ToArray();
        }

        private IInstaller ResolveInstaller(Type installerType)
        {
            IActivator activator = new ActivatorExpression();
            var constructor = installerType.GetConstructor(new Type[0]);

            return (IInstaller)activator.CreateInstance(constructor, new object[0]);
        }
    }
}