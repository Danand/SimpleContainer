using SimpleContainer.Unity.Roots;

using UnityEngine;

namespace SimpleContainer.Unity.Installers
{
    public sealed class MonoRegistrator : MonoInstaller
    {
        public MonoBehaviour[] components;
        public bool laterThanRoot;

        void Awake()
        {
            if (laterThanRoot)
            {
                var projectRoot = FindObjectOfType<UnityProjectRootBase>();
                projectRoot.LateInstall(this);
            }
        }

        public override void Install(Container container)
        {
            foreach (var component in components)
            {
                var type = component.GetType();
                container.Register(type, type, Scope.Transient, component);
            }
        }
    }
}