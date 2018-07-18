using NUnit.Framework;

using SimpleContainer.Tests.DummyTypes;

namespace SimpleContainer.Tests
{
    [TestFixture]
    public class SimpleContainerTests
    {
        [Test]
        public void Resolve_Transient()
        {
            var container = new SimpleContainer();

            container.Register<ICat, GingerCat>(Scope.Transient);

            var catFirst = container.Resolve<ICat>();
            var catSecond = container.Resolve<ICat>();

            Assert.AreNotSame(catFirst, catSecond);
        }

        [Test]
        public void Resolve_Singleton()
        {
            var container = new SimpleContainer();

            container.Register<ICelebrityCat, CelebrityCat>(Scope.Singleton);

            var catFirst = container.Resolve<ICelebrityCat>();
            var catSecond = container.Resolve<ICelebrityCat>();

            Assert.AreSame(catFirst, catSecond);
        }

        [Test]
        public void Resolve_Factory()
        {
            var container = new SimpleContainer();

            container.RegisterFactory<CatFactory>();

            var catFactory = container.Resolve<CatFactory>();
            var cat = catFactory.Create();

            Assert.IsInstanceOf<ICat>(cat);
        }
    }
}