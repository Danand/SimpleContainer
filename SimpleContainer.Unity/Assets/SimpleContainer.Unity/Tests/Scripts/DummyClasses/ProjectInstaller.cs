using SimpleContainer.Unity.Installers;

namespace SimpleContainer.Unity.Tests.DummyClasses
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void Install(Container container)
        {
            container.Register<IDialogueManager, DialogueManager>(Scope.Singleton);
            container.Register<ILocalizationRepository, LocalizationRepository>(Scope.Singleton);
        }
    }
}
