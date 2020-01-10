using System;

using SimpleContainer.Unity.Example.Dependent.Interfaces;

namespace SimpleContainer.Unity.Example.Dependent.CultureInfoProviders
{
    public sealed class CultureInfoFormatterTimestamp : ICultureInfoFormatter
    {
        string ICultureInfoFormatter.FormatDateTime(DateTime dateTime)
        {
            TimeSpan span = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));
            return span.TotalSeconds.ToString("N0");
        }
    }
}