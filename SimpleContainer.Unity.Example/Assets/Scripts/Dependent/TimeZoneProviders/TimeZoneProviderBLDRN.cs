using System;

using SimpleContainer.Unity.Example.Dependent.Interfaces;

namespace SimpleContainer.Unity.Example.Dependent.TimeZoneProviders
{
    public sealed class TimeZoneProviderBLDRN : ITimeZoneProvider
    {
        string ITimeZoneProvider.Name => "BLDRNR";

        DateTime ITimeZoneProvider.MoveDateTime(DateTime sourceDateTime)
        {
            return new DateTime(2049, sourceDateTime.Month, sourceDateTime.Day, sourceDateTime.Hour, sourceDateTime.Minute, sourceDateTime.Second);
        }
    }
}