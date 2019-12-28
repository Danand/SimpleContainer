namespace SimpleContainer.Tests.DummyTypes
{
    public sealed class CarFourWheelDrive : ICar
    {
        private readonly IEngine engine;

        public CarFourWheelDrive(IEngine engine)
        {
            this.engine = engine;
        }

        IEngine ICar.Engine
        {
            get { return engine; }
        }
    }
}