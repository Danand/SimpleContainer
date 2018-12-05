using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using SimpleContainer.Exceptions;
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

#if NET35
        public void ThrowIfNotResolved()
        {
            foreach (var binding in bindings)
            {
                var cachedInstances = binding.Value.GetCachedInstances();

                if (cachedInstances.Length == 0)
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

                if (cachedInstances.Length == 0)
                    exceptions.Add(new TypeNotResolvedException(binding.Key));
            }

            if (exceptions.Count > 0)
                throw new AggregateException(exceptions);
        }
#endif

        internal object[] GetAllCached()
        {
            return bindings.SelectMany(resolver => resolver.Value.GetCachedInstances()).ToArray();
        }

        private void Initialize(object[] results)
        {
            foreach (var result in results)
            {
                if (result is IInitializible initializible)
                    initializible.Initialize();
            }
        }
    }
}