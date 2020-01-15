using System;
using System.Globalization;

using SimpleContainer.Unity.Example.Dependent.Interfaces;

namespace SimpleContainer.Unity.Example.Dependent.CultureInfoProviders
{
    public sealed class CultureInfoFormatterUS : ICultureInfoFormatter
    {
        string ICultureInfoFormatter.FormatDateTime(DateTime dateTime)
        {
            return dateTime.ToString(new CultureInfo("en-US"));
        }
    }
}