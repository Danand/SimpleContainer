using System;
using System.Globalization;

using SimpleContainer.Unity.Example.Dependent.Interfaces;

namespace SimpleContainer.Unity.Example.Dependent.CultureInfoProviders
{
    public sealed class CultureInfoFormatterJP : ICultureInfoFormatter
    {
        string ICultureInfoFormatter.FormatDateTime(DateTime dateTime)
        {
            return DateTime.Now.ToString("F", new CultureInfo("ja-JP"));
        }
    }
}