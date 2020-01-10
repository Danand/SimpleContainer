// Perfect. No references to DI-container here!

using System;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

using SimpleContainer.Unity.Example.Dependent.Interfaces;

namespace SimpleContainer.Unity.Example.Dependent
{
    public sealed class UIManager : MonoBehaviour
    {
        public Dropdown dropdownImplementation;
        public Text labelTime;

        private int implementationIndex;

        [Inject]
        public ITimeZoneProvider[] TimeZoneProviders { get; set; }

        [Inject]
        public ICultureInfoFormatter CultureInfoFormatter { get; set; }

        void OnEnable()
        {
            dropdownImplementation.onValueChanged.AddListener(OnDropdownValueChanged);
        }

        void OnDisable()
        {
            dropdownImplementation.onValueChanged.RemoveListener(OnDropdownValueChanged);
        }

        /// <summary>
        /// Should be called after dependencies was resolved.
        /// </summary>
        public void Initialize()
        {
            var options = TimeZoneProviders.Select(zoneProvider => zoneProvider.Name).ToList();

            dropdownImplementation.ClearOptions();
            dropdownImplementation.AddOptions(options);

            OnClickReload();
        }

        public void OnClickReload()
        {
            var dateTime = DateTime.UtcNow;
            var movedDateTime = TimeZoneProviders[implementationIndex].MoveDateTime(dateTime);
            var dateTimeString = CultureInfoFormatter.FormatDateTime(movedDateTime);

            labelTime.text = dateTimeString;
        }

        private void OnDropdownValueChanged(int value)
        {
            implementationIndex = value;
        }
    }
}
