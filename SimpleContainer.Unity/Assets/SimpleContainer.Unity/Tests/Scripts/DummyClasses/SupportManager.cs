namespace SimpleContainer.Unity.Tests.DummyClasses
{
    public class SupportManager : ISupportManager
    {
        public ILocalizationRepository LocalizationRepository { get; }

        public SupportManager(ILocalizationRepository localizationRepository)
        {
            LocalizationRepository = localizationRepository;
        }
    }
}