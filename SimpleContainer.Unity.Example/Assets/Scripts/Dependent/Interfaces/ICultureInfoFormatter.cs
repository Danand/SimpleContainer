using System;

namespace SimpleContainer.Unity.Example.Dependent.Interfaces
{
    public interface ICultureInfoFormatter
    {
        string FormatDateTime(DateTime dateTime);
    }
}