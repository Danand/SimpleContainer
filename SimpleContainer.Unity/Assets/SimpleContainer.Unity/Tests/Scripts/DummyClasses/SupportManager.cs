using UnityEngine;

namespace SimpleContainer.Unity.Tests.DummyClasses
{
    public class SupportManager : MonoBehaviour, ISupportManager
    {
        public ILocalizationRepository LocalizationRepository { get; }

        public SupportManager(ILocalizationRepository localizationRepository)
        {
            LocalizationRepository = localizationRepository;
        }
    }
}