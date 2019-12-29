using System;

using SimpleContainer.Unity.Events;
using SimpleContainer.Unity.Installers;

using UnityEngine;

namespace SimpleContainer.Unity
{
    public class UnityProjectRoot : MonoBehaviour
    {
        public MonoInstaller[] installers;

        private Container container;

        async void Awake()
        {
            container = Container.Create();

            container.Install(new MonoCallbacksInstaller());

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

        void OnDestroy()
        {
            ((IDisposable)container).Dispose();
        }

        void Update()
        {
            container.SendEvent(new UpdateArgs());
        }
    }
}