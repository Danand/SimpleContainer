namespace SimpleContainer.Tests.DummyTypes
{
    public class Robot : IRobot
    {
        public Robot(IRobotLegs robotLegs)
        {
            Legs = robotLegs;
        }

        public IRobotLegs Legs { get; set; }
    }
}