namespace SimpleContainer.Tests.DummyTypes
{
    public sealed class EngineMedium : IEngine
    {
        private readonly IPhysics physics;

        public EngineMedium(IPhysics physics)
        {
            this.physics = physics;
        }

        IPhysics IEngine.Physics
        {
            get { return physics; }
        }
    }
}