using System.Threading.Tasks;

using SimpleContainer.Interfaces;

using UnityEngine;

namespace SimpleContainer.Unity
{
    public abstract class MonoInstaller : MonoBehaviour, IInstaller
    {
        public abstract void Install(Container container);

        public virtual void Resolve(Container container) { }

        public virtual Task ResolveAsync(Container container)
        {
            return Task.CompletedTask;
        }

        public virtual Task AfterResolveAsync(Container container)
        {
            return Task.CompletedTask;
        }
    }
}