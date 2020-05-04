using System.Threading.Tasks;

using SimpleContainer.Unity.Installers;

namespace SimpleContainer.Unity.Tests.DummyClasses
{
    public sealed class ProjectInstaller : MonoInstaller
    {
        public override void Install(Container container)
        {
            container.Register<IDialogueManager, DialogueManager>(Scope.Singleton);
            container.Register<ILocalizationRepository, LocalizationRepository>(Scope.Singleton);
        }

        public override Task ResolveAsync(Container container)
        {
            container.Resolve<IDialogueManager>();
            container.Resolve<ILocalizationRepository>();

            return Task.CompletedTask;
        }
    }
}
