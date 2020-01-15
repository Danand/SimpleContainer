using System;

using SimpleContainer.Unity.Example.Dependent.Interfaces;

namespace SimpleContainer.Unity.Example.Dependent.TimeZoneProviders
{
    public sealed class TimeZoneProviderDefault : ITimeZoneProvider
    {
        string ITimeZoneProvider.Name => "SELECT TIME IMPLEMENTATION";

        DateTime ITimeZoneProvider.MoveDateTime(DateTime sourceDateTime)
        {
            return sourceDateTime;
        }
    }
}