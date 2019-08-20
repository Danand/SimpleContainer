namespace SimpleContainer.Tests.DummyTypes
{
    public interface IEngine
    {
        IPhysics Physics { get; }
        IPhysics PhysicsFromFieldPublic { get; }
        IPhysics PhysicsFromFieldPrivate { get; }
        IPhysics PhysicsFromPropertyPublic { get; }
        IPhysics PhysicsFromPropertyPrivate { get; }
        IPhysics PhysicsFromMethodPublic { get; }
        IPhysics PhysicsFromMethodPrivate { get; }
        IPhysics[] PhysicsFromMethodPublicMultiple { get; }
    }
}