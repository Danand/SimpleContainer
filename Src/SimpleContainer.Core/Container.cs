using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using SimpleContainer.Attributes;
using SimpleContainer.Exceptions;
using SimpleContainer.Interfaces;

namespace SimpleContainer
{
    public sealed partial class Container : IDisposable
    {
        internal readonly Dictionary<Type, ConstructorInfo> cachedConstructors = new Dictionary<Type, ConstructorInfo>();

        internal Type injectAttributeType = typeof(InjectAttribute);

        private readonly Dispatcher dispatcher = new Dispatcher();
        private readonly Dictionary<Type, Resolver> bindings = new Dictionary<Type, Resolver>();

        public static Container Create()
        {
            return new Container();
        }

        public void Install(params IInstaller[] installers)
        {
            foreach (var installer in installers)
                installer.Install(this);
        }

        public void Install(params Type[] installerTypes)
        {
            foreach (var installerType in installerTypes)
            {
                var installer = ResolveInstaller(installerType);
                installer.Install(this);
            }
        }

        public void Install(Assembly assembly, params string[] installerNames)
        {
            foreach (var installerName in installerNames)
            {
                var installerType = assembly.GetType(installerName, true);
                var installer = ResolveInstaller(installerType);

                installer.Install(this);
            }
        }

        public void OverrideFrom(Container other)
        {
            foreach (var binding in other.bindings)
                bindings[binding.Key] = binding.Value.CopyToContainer(this);

            injectAttributeType = other.injectAttributeType;
        }

        public void InjectIntoRegistered()
        {
            foreach (var binding in bindings)
            {
                var cachedInstances = binding.Value.GetCachedInstances();

                foreach (var cachedInstance in cachedInstances)
                    binding.Value.InjectIntoInstance(cachedInstance.Value);
            }
        }

#if NET35
        public void ThrowIfNotResolved()
        {
            foreach (var binding in bindings)
            {
                var cachedInstances = binding.Value.GetCachedInstances();

                if (!cachedInstances.GetEnumerator().MoveNext())
                    throw new TypeNotResolvedException(binding.Key);
            }
        }
#else
        public void ThrowIfNotResolved()
        {
            var exceptions = new List<Exception>();

            foreach (var binding in bindings)
            {
                var cachedInstances = binding.Value.GetCachedInstances();

                if (!cachedInstances.GetEnumerator().MoveNext())
                    exceptions.Add(new TypeNotResolvedException(binding.Key));
            }

            if (exceptions.Count > 0)
                throw new AggregateException(exceptions);
        }
#endif

        internal IEnumerable<InstanceWrapper> GetAllCached()
        {
            return bindings.SelectMany(resolver => resolver.Value.GetCachedInstances());
        }

        void IDisposable.Dispose()
        {
            foreach (var resolver in bindings.Values)
                resolver.DisposeInstances();
        }

        private void Initialize(InstanceWrapper[] instances)
        {
            foreach (var instance in instances)
            {
                if (instance.Value is IInitializible initializible && !instance.IsInitialized)
                {
                    initializible.Initialize();
                    instance.IsInitialized = true;
                }
            }
        }
    }
}