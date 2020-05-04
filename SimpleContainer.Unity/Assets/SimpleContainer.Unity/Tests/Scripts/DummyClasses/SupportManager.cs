using UnityEngine;

namespace SimpleContainer.Unity.Tests.DummyClasses
{
    public sealed class SupportManager : MonoBehaviour, ISupportManager
    {
        [Inject]
        public ILocalizationRepository LocalizationRepository { get; set; }
    }
}