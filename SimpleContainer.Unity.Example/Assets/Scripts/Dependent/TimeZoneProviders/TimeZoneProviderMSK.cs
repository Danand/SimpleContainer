using System;

using SimpleContainer.Unity.Example.Dependent.Interfaces;

namespace SimpleContainer.Unity.Example.Dependent.TimeZoneProviders
{
    public sealed class TimeZoneProviderMSK : ITimeZoneProvider
    {
        string ITimeZoneProvider.Name => "MSK";

        DateTime ITimeZoneProvider.MoveDateTime(DateTime sourceDateTime)
        {
            // TimeZoneInfo, blah-blah-blah...
            return sourceDateTime.AddHours(3);
        }
    }
}