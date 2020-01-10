using System;

namespace SimpleContainer.Unity.Example.Dependent.Interfaces
{
    public interface ITimeZoneProvider
    {
        string Name { get; }

        DateTime MoveDateTime(DateTime sourceDateTime);
    }
}