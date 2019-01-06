using SimpleContainer.Attributes;

namespace SimpleContainer.Tests.DummyTypes
{
    public sealed class EngineMedium : IEngine
    {
        [Inject]
        public IPhysics physicsFromFieldPublic;

        private readonly IPhysics physics;

#pragma warning disable 649
        [Inject]
        private IPhysics physicsFromFieldPrivate;
#pragma warning restore 649

        public EngineMedium(IPhysics physics)
        {
            this.physics = physics;
        }

        IPhysics IEngine.Physics
        {
            get { return physics; }
        }

        IPhysics IEngine.PhysicsFromFieldPublic
        {
            get { return physicsFromFieldPublic; }
        }

        IPhysics IEngine.PhysicsFromFieldPrivate
        {
            get { return physicsFromFieldPrivate; }
        }

        IPhysics IEngine.PhysicsFromPropertyPublic
        {
            get { return PhysicsFromPropertyPublicBacking; }
        }

        IPhysics IEngine.PhysicsFromPropertyPrivate
        {
            get { return PhysicsFromPropertyPrivateBacking; }
        }

        public IPhysics PhysicsFromMethodPublic { get; private set; }

        public IPhysics PhysicsFromMethodPrivate { get; private set; }

        [Inject]
        public IPhysics PhysicsFromPropertyPublicBacking { get; set; }

        [Inject]
        private IPhysics PhysicsFromPropertyPrivateBacking { get; set; }

        [Inject]
        public void DummyInjectMethodPublic(IPhysics physics)
        {
            PhysicsFromMethodPublic = physics;
        }

        [Inject]
        private void DummyInjectMethodPrivate(IPhysics physics)
        {
            PhysicsFromMethodPrivate = physics;
        }
    }
}