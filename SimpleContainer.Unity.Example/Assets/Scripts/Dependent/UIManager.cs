// Perfect. No references to DI-container here!

using System;
using System.Linq;
using System.Threading;

using UnityEngine;
using UnityEngine.UI;

using SimpleContainer.Unity.Example.Dependent.Interfaces;
using SimpleContainer.Unity.Example.Extensions;

using ILogger = SimpleContainer.Unity.Example.Dependent.Interfaces.ILogger;

namespace SimpleContainer.Unity.Example.Dependent
{
    public sealed class UIManager : MonoBehaviour
    {
        public Dropdown dropdownImplementation;
        public Text labelTime;

        private readonly CancellationTokenSource cts = new CancellationTokenSource();

        private int timeZoneIndex;

        [Inject]
        public ITimeZoneProvider[] TimeZoneProviders { get; set; }

        [Inject]
        public ICultureInfoFormatter CultureInfoFormatter { get; set; }

        [Inject]
        public ILogger Logger { get; set; }

        private DateTime DateTimeNow => DateTime.UtcNow;

        void OnEnable()
        {
            dropdownImplementation.onValueChanged.AddListener(OnDropdownValueChanged);
        }

        void OnDisable()
        {
            dropdownImplementation.onValueChanged.RemoveListener(OnDropdownValueChanged);
        }

        void OnDestroy()
        {
            cts.Cancel();
        }

        /// <summary>
        /// Should be called after dependencies was resolved.
        /// </summary>
        public void Initialize()
        {
            var options = TimeZoneProviders.Select(zoneProvider => zoneProvider.Name).ToList();

            dropdownImplementation.ClearOptions();
            dropdownImplementation.AddOptions(options);

            _ = this.ReactAsync(manager => manager.DateTimeNow, ShowFormattedTime, cts);
        }

        private void OnDropdownValueChanged(int value)
        {
            Logger.LogInfo($"Timezone index changed to {value}");
            timeZoneIndex = value;
            ShowFormattedTime(DateTime.UtcNow);
        }

        private void ShowFormattedTime(DateTime dateTime)
        {
            var movedDateTime = TimeZoneProviders[timeZoneIndex].MoveDateTime(dateTime);
            var dateTimeString = CultureInfoFormatter.FormatDateTime(movedDateTime);

            labelTime.text = dateTimeString;
        }
    }
}
