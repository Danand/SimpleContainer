namespace SimpleContainer.Unity.Tests.DummyClasses
{
    public sealed class DialogueManager : IDialogueManager
    {
        private readonly ILocalizationRepository localizationRepository;

        public DialogueManager(ILocalizationRepository localizationRepository)
        {
            this.localizationRepository = localizationRepository;
        }
    }
}