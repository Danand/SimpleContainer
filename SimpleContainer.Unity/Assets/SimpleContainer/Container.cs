using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

using SimpleContainer.Exceptions;
using SimpleContainer.Interfaces;

[assembly: InternalsVisibleTo("SimpleContainer.Tests")]
namespace SimpleContainer
{
    public sealed partial class Container
    {
        private Container() { }

        internal DependencyManager DependencyManager { get; set; }

        internal HashSet<Type> InjectAttributeTypes { get; set; } = new HashSet<Type>();

        public static Container Create()
        {
            var container = new Container();
            var dependencyManager = new DependencyManager(container);

            container.DependencyManager = dependencyManager;

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
            foreach (var rootNode in other.DependencyManager.RootNodes)
            {
                var nodeToRemove = DependencyManager.RootNodes.FirstOrDefault(node => node.ContractType == rootNode.ContractType);

                if (nodeToRemove != null)
                {
                    DependencyManager.RootNodes.Remove(nodeToRemove);
                    DependencyManager.RootNodes.Add(rootNode);
                }
            }

            foreach (var injectAttributeType in other.InjectAttributeTypes)
            {
                InjectAttributeTypes.Add(injectAttributeType);
            }
        }

        public void ThrowIfNotResolved()
        {
            var exceptions = new List<Exception>();

            foreach (var rootNode in DependencyManager.RootNodes)
            {
                if (rootNode.Instance == null)
                    exceptions.Add(new TypeNotResolvedException(rootNode.ContractType));
            }

            if (exceptions.Count > 0)
                throw new AggregateException(exceptions);
        }

        internal IEnumerable<object> GetAllCached()
        {
            return DependencyManager.RootNodes.Select(rootNode => rootNode.Instance);
        }

        public void InjectIntoRegistered()
        {
            DependencyManager.InjectIntoRegistered();
        }
    }
}