using SimpleContainer.Unity.Installers;

namespace SimpleContainer.Unity.Tests.DummyClasses
{
    public sealed class SceneInstaller : MonoInstaller
    {
        public SupportManager supportManager;

        public override void Install(Container container)
        {
            container.RegisterAttribute<InjectAttribute>();
            container.Register<ISupportManager>(Scope.Singleton, supportManager);
        }
    }
}
