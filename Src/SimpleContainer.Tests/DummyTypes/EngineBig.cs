using System;

namespace SimpleContainer.Tests.DummyTypes
{
    public sealed class EngineBig : IEngine
    {
        IPhysics IEngine.Physics
        {
            get { throw new NotImplementedException(); }
        }
    }
}