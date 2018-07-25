namespace SimpleContainer.Tests.DummyTypes
{
    public sealed class CarFourWheelDrive : ICar
    {
        private readonly IEngine _engine;

        public CarFourWheelDrive(IEngine engine)
        {
            _engine = engine;
        }

        IEngine ICar.Engine
        {
            get { return _engine; }
        }
    }
}