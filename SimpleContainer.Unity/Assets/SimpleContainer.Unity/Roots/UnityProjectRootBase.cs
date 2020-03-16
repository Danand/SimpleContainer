using System.Threading.Tasks;

using SimpleContainer.Unity.Installers;

using UnityEngine;

namespace SimpleContainer.Unity.Roots
{
    public abstract class UnityProjectRootBase : MonoBehaviour
    {
        public MonoInstaller[] installers;

        private Container container;

        protected async Task InstallAsyncInternally()
        {
            container = Container.Create();

            foreach (MonoInstaller installer in installers)
                container.Install(installer);

            var registrators = Resources.FindObjectsOfTypeAll<MonoRegistrator>();

            foreach (MonoRegistrator registrator in registrators)
            {
                if (!registrator.laterThanRoot)
                    container.Install(registrator);
            }

            foreach (MonoInstaller installer in installers)
                await installer.ResolveAsync(container);

            container.InjectIntoRegistered();

            container.ThrowIfNotResolved();

            foreach (MonoInstaller installer in installers)
                await installer.AfterResolveAsync(container);
        }

        internal void LateInstall(MonoInstaller installer)
        {
            container.Install(installer);
            container.InjectIntoRegistered();
        }
    }
}