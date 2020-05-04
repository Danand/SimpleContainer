namespace SimpleContainer.Tests.DummyTypes
{
    public class RobotHolder : IRobotHolder
    {
        public RobotHolder(IRobot robot)
        {
            Robot = robot;
        }

        public IRobot Robot { get; set; }
    }
}