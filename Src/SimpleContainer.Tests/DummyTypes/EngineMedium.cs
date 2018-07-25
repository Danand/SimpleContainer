namespace SimpleContainer.Tests.DummyTypes
{
    public sealed class EngineMedium : IEngine
    {
        private readonly IPhysics _physics;

        public EngineMedium(IPhysics physics)
        {
            _physics = physics;
        }

        IPhysics IEngine.Physics
        {
            get { return _physics; }
        }
    }
}