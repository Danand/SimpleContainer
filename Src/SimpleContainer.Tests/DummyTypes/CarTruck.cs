using System;

namespace SimpleContainer.Tests.DummyTypes
{
    public sealed class CarTruck : ICar
    {
        IEngine ICar.Engine
        {
            get { throw new NotImplementedException(); }
        }
    }
}