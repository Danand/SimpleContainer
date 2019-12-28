using System;

namespace SimpleContainer.Tests.DummyTypes
{
    public sealed class EngineBig : IEngine
    {
        IPhysics IEngine.Physics
        {
            get { throw new NotImplementedException(); }
        }

        IPhysics IEngine.PhysicsFromFieldPublic
        {
            get { throw new NotImplementedException(); }
        }

        IPhysics IEngine.PhysicsFromFieldPrivate
        {
            get { throw new NotImplementedException(); }
        }

        IPhysics IEngine.PhysicsFromPropertyPublic
        {
            get { throw new NotImplementedException(); }
        }

        IPhysics IEngine.PhysicsFromPropertyPrivate
        {
            get { throw new NotImplementedException(); }
        }

        IPhysics IEngine.PhysicsFromMethodPublic
        {
            get { throw new NotImplementedException(); }
        }

        IPhysics IEngine.PhysicsFromMethodPrivate
        {
            get { throw new NotImplementedException(); }
        }

        IPhysics[] IEngine.PhysicsFromMethodPublicMultiple
        {
            get { throw new NotImplementedException(); }
        }
    }
}