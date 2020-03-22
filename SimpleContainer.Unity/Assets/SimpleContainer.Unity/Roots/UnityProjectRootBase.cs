using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

using SimpleContainer.Installers;
using SimpleContainer.Interfaces;
using SimpleContainer.Unity.Installers;

using UnityEngine;

namespace SimpleContainer.Unity.Roots
{
    public abstract class UnityProjectRootBase : MonoBehaviour
    {
        public MonoInstaller[] installers;

        private readonly Queue<IInstaller> installersQueue = new Queue<IInstaller>();

        private Container container;

        protected virtual void Awake()
        {
            foreach (var monoInstaller in installers)
            {
                installersQueue.Enqueue(monoInstaller);
            }
        }

        public void AddInstaller(IInstaller installer)
        {
            installersQueue.Enqueue(installer);
        }

        public void AddInstaller(Assembly assembly, string installerName)
        {
            installersQueue.Enqueue(new StringInstaller(assembly, installerName));
        }

        protected async Task InstallAsyncInternally()
        {
            container = Container.Create();

            while (installersQueue.Count > 0)
            {
                container.Install(installersQueue.Dequeue());
            }

            var registrators = Resources.FindObjectsOfTypeAll<MonoRegistrator>();

            foreach (MonoRegistrator registrator in registrators)
            {
                if (!registrator.laterThanRoot)
                    container.Install(registrator);
            }

            foreach (MonoInstaller installer in installers)
            {
                await installer.ResolveAsync(container);
            }

            container.InjectIntoRegistered();

            container.ThrowIfNotResolved();

            foreach (MonoInstaller installer in installers)
            {
                await installer.AfterResolveAsync(container);
            }
        }

        internal void InstallMonoRegistrator(MonoInstaller installer)
        {
            container.Install(installer);
            container.InjectIntoRegistered();
        }
    }
}