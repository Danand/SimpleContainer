using System;

using SimpleContainer.Unity.Example.Dependent.Interfaces;

namespace SimpleContainer.Unity.Example.Dependent.TimeZoneProviders
{
    public sealed class TimeZoneProviderBTTF : ITimeZoneProvider
    {
        string ITimeZoneProvider.Name => "BTTF";

        DateTime ITimeZoneProvider.MoveDateTime(DateTime sourceDateTime)
        {
            return new DateTime(1985, 10, 26, sourceDateTime.Hour, sourceDateTime.Minute, sourceDateTime.Second);
        }
    }
}