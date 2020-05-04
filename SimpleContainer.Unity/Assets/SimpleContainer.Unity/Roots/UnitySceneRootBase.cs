using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

using SimpleContainer.Installers;
using SimpleContainer.Interfaces;
using SimpleContainer.Unity.Installers;
using SimpleContainer.Unity.Utils;

using UnityEngine;

namespace SimpleContainer.Unity.Roots
{
    public abstract class UnitySceneRootBase : MonoBehaviour
    {
        public MonoInstaller[] installers;

        private readonly Queue<IInstaller> installersQueue = new Queue<IInstaller>();

        public Container Container { get; private set; }

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
            var sceneContainer = Container.Create();

            while (installersQueue.Count > 0)
            {
                sceneContainer.Install(installersQueue.Dequeue());
            }

            var registrators = Resources.FindObjectsOfTypeAll<MonoRegistrator>();

            foreach (MonoRegistrator registrator in registrators)
            {
                if (!registrator.laterThanRoot)
                    sceneContainer.Install(registrator);
            }

            Container projectContainer =  null;

            var projectRoot = ComponentUtils.FindInAllLoadedScenes<UnityProjectRootBase>();

            if (projectRoot != null)
            {
                projectContainer = projectRoot.Container;
                projectContainer.OverrideFrom(sceneContainer);
            }

            Container = projectContainer ?? sceneContainer;

            foreach (MonoInstaller installer in installers)
            {
                await installer.ResolveAsync(Container);
            }

            Container.InjectIntoRegistered();

            Container.ThrowIfNotResolved();

            foreach (MonoInstaller installer in installers)
            {
                await installer.AfterResolveAsync(Container);
            }
        }

        internal void InstallMonoRegistrator(MonoInstaller installer)
        {
            Container.Install(installer);
            Container.InjectIntoRegistered();
        }
    }
}