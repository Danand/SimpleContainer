using System.Threading.Tasks;

using SimpleContainer.Unity.Example.Dependent;
using SimpleContainer.Unity.Installers;

namespace SimpleContainer.Unity.Example.Installers
{
    public sealed class PresentationInstaller : MonoInstaller
    {
        public UIManager uiManager;

        public override void Install(Container container)
        {
            container.RegisterAttribute<InjectAttribute>();
            container.Register(Scope.Singleton, uiManager);
        }

        public override Task AfterResolveAsync(Container container)
        {
            uiManager.Initialize();
            return Task.CompletedTask;
        }
    }
}
