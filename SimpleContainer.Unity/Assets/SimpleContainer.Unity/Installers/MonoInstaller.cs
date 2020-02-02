using System.Threading.Tasks;

using SimpleContainer.Interfaces;
using SimpleContainer.Unity.Roots;

using UnityEngine;

namespace SimpleContainer.Unity.Installers
{
    /// <summary>
    /// Unity-specific implementation of installer.
    /// Contains methods for running a composition root.
    /// </summary>
    public abstract class MonoInstaller : MonoBehaviour, IInstaller
    {
        /// <summary>
        /// Registers contract/result types and instances.
        /// </summary>
        public abstract void Install(Container container);

        /// <summary>
        /// Method for non-lazy resolvings calls.
        /// May be required to avoid <see cref="Exceptions.TypeNotRegisteredException"/>
        /// at <see cref="Container.ThrowIfNotResolved()"/>
        /// in situations when contract dependency was not declared.
        /// </summary>
        public virtual Task ResolveAsync(Container container)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// The point where all resolvings at <see cref="UnityProjectRootBase.Awake()"/> is guaranteed.
        /// </summary>
        public virtual Task AfterResolveAsync(Container container)
        {
            return Task.CompletedTask;
        }
    }
}