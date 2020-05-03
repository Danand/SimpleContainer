using SimpleContainer.Unity.Installers;

namespace SimpleContainer.Unity.Tests.DummyClasses
{
    public class SceneInstaller : MonoInstaller
    {
        public SupportManager supportManager;

        public override void Install(Container container)
        {
            container.Register<ISupportManager>(Scope.Singleton, supportManager);
        }
    }
}
