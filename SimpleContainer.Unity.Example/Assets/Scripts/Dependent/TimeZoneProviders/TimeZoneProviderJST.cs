using System;

using SimpleContainer.Unity.Example.Dependent.Interfaces;

namespace SimpleContainer.Unity.Example.Dependent.TimeZoneProviders
{
    public sealed class TimeZoneProviderJST : ITimeZoneProvider
    {
        string ITimeZoneProvider.Name => "JST";

        DateTime ITimeZoneProvider.MoveDateTime(DateTime sourceDateTime)
        {
            return sourceDateTime.AddHours(9);
        }
    }
}