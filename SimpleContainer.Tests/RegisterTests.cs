using NUnit.Framework;

using SimpleContainer.Tests.DummyTypes;

namespace SimpleContainer.Tests
{
    [TestFixture]
    public class RegisterTests
    {
        [Test]
        public void Register_Instance_Singleton_NotThrows()
        {
            var container = Container.Create();
            var instance = new ColorRed();

            container.Register(Scope.Singleton, instance);

            container.ThrowIfNotResolved();

            Assert.Pass();
        }

        [Test]
        public void Register_Instance_Transient_NotThrows()
        {
            var container = Container.Create();
            var instanceBlue = new ColorBlue();

            container.Register(Scope.Transient, instanceBlue);

            container.ThrowIfNotResolved();

            Assert.Pass();
        }

        [Test]
        public void Register_Instances_Transient_Unique()
        {
            var container = Container.Create();

            var instanceRedFirst = new ColorRed();
            var instanceRedSecond = new ColorRed();
            var instanceRedThird = new ColorRed();

            container.Register(Scope.Transient, instanceRedFirst);
            container.Register(Scope.Transient, instanceRedSecond);
            container.Register(Scope.Transient, instanceRedThird);

            var reds = container.GetCachedMultiple<ColorRed>();

            Assert.AreEqual(3, reds.Length);
            CollectionAssert.AllItemsAreUnique(reds);
        }

        [Test]
        public void Register_String_Singleton_NotThrows()
        {
            var container = Container.Create();

            container.Register(nameof(IColor), nameof(ColorRed), Scope.Singleton);
            container.Resolve<IColor>();
            container.ThrowIfNotResolved();

            Assert.Pass();
        }
    }
}