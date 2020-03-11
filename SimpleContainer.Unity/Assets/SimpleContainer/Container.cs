using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using SimpleContainer.Activators;
using SimpleContainer.Attributes;
using SimpleContainer.Exceptions;
using SimpleContainer.Interfaces;

namespace SimpleContainer
{
    public sealed partial class Container : IDisposable
    {
        internal readonly HashSet<Type> injectAttributeTypes = new HashSet<Type> { typeof(InjectAttribute) };

        private readonly Dispatcher dispatcher = new Dispatcher();
        private readonly Dictionary<Type, Resolver> bindings = new Dictionary<Type, Resolver>();

        private Container() { }

        public static Container Create()
        {
            var container = new Container();
            container.InstallResolver();
            return container;
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

            foreach (var injectAttributeType in other.injectAttributeTypes )
                injectAttributeTypes.Add(injectAttributeType);
        }

        public void InjectIntoRegistered()
        {
            foreach (var binding in bindings)
            {
                var cachedInstances = binding.Value.GetCachedInstances();

                foreach (var cachedInstance in cachedInstances)
                    binding.Value.InjectIntoInstance(cachedInstance);
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

        internal IEnumerable<object> GetAllCached()
        {
            return bindings.SelectMany(resolver => resolver.Value.GetCachedInstances());
        }

        void IDisposable.Dispose()
        {
            foreach (var resolver in bindings.Values)
                resolver.DisposeInstances();
        }

        private void InstallResolver()
        {
            var resolver = CreateInitialResolver();

            resolver.Initialize(
                container:      this,
                resultTypes:    new Type[] { typeof(Resolver) }, 
                scope:          Scope.Transient,
                instances:      null,
                args:           new object[0]);

            bindings.Add(typeof(Resolver), resolver);

            resolver.SetMethod(CreateInitialResolver);

            Install(new InternalInstaller());

            resolver.RemoveMethod();
        }

        private Resolver CreateInitialResolver()
        {
            var constructorCacher = new ConstructorCacher();
            var resolver = new Resolver(new ActivatorReflection(constructorCacher), constructorCacher);

            return resolver;
        }

        private string GetBindingsString(Dictionary<Type, Resolver> bindings)
        {
            return string.Join($",{Environment.NewLine}", bindings.Keys.Select(key => key.Name));
        }
    }
}