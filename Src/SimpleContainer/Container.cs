using System;
using System.Collections.Generic;

using SimpleContainer.Interfaces;

namespace SimpleContainer
{
    public sealed partial class Container
    {
        private readonly Dispatcher dispatcher = new Dispatcher();
        private readonly Dictionary<Type, Resolver> bindings = new Dictionary<Type, Resolver>();

        public static Container Create()
        {
            return new Container();
        }

        public void Dispose()
        {
            foreach (var resolver in bindings.Values)
                resolver.DisposeInstances();
        }

        public void Install(params IInstaller[] installers)
        {
            foreach (var installer in installers)
                installer.Install(this);
        }

        internal object[] GetAllCached<TContract>()
        {
            var contractType = typeof(TContract);

            if (!bindings.TryGetValue(contractType, out var resolver))
                throw new TypeNotRegisteredException(contractType);

            return resolver.GetCachedInstances();
        }

        private void Initialize(object result)
        {
            if (result is IInitializible initializible)
                initializible.Initialize();
        }
    }
}