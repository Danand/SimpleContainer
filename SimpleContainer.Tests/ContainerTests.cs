using NUnit.Framework;

using SimpleContainer.Tests.DummyTypes;

namespace SimpleContainer.Tests
{
    [TestFixture]
    public sealed class ContainerTests
    {
        [Test]
        public void OverrideFrom_Unresolved_Pass()
        {
            var containerFirst = Container.Create();
            var containerSecond = Container.Create();

            containerFirst.Register<IRobotHolder, RobotHolder>(Scope.Singleton);
            containerFirst.Register<IRobot, Robot>(Scope.Singleton);
            containerFirst.Register<IRobotLegs, RobotLegsRegular>(Scope.Singleton);

            containerSecond.Register<IRobotLegs, RobotLegsCaterpillar>(Scope.Singleton);

            containerFirst.OverrideFrom(containerSecond);

            var result = containerFirst.Resolve<IRobotHolder>();

            Assert.AreEqual(typeof(RobotLegsCaterpillar), result.Robot.Legs.GetType());
        }

        [Test]
        public void OverrideFrom_Resolved_Pass()
        {
            var containerFirst = Container.Create();
            var containerSecond = Container.Create();

            containerFirst.Register<IRobotHolder, RobotHolder>(Scope.Transient);
            containerFirst.Register<IRobot, Robot>(Scope.Transient);
            containerFirst.Register<IRobotLegs, RobotLegsRegular>(Scope.Transient);

            var resolvedFirst = containerFirst.Resolve<IRobotHolder>();

            containerSecond.Register<IRobotLegs, RobotLegsCaterpillar>(Scope.Transient);

            containerFirst.OverrideFrom(containerSecond);

            var resolvedSecond = containerFirst.Resolve<IRobotHolder>();

            Assert.AreNotEqual(resolvedFirst.Robot.Legs.GetType(), resolvedSecond.Robot.Legs.GetType());
        }
    }
}