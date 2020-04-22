namespace SimpleContainer.Unity.Tests.DummyClasses
{
    public class DialogueManager : IDialogueManager
    {
        private readonly ILocalizationRepository localizationRepository;

        public DialogueManager(ILocalizationRepository localizationRepository)
        {
            this.localizationRepository = localizationRepository;
        }
    }
}