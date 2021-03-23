using System.Linq;

using SimpleContainer.Tests.DummyTypes;
using SimpleContainer.Exceptions;

using NUnit.Framework;

namespace SimpleContainer.Tests
{
    [TestFixture]
    public class ResolveTests
    {
        [Test]
        public void Resolve_Transient()
        {
            var container = Container.Create();

            container.RegisterAttribute<InjectAttribute>();
            container.Register<ICar, CarTruck>(Scope.Transient);

            var carFirst = container.Resolve<ICar>();
            var carSecond = container.Resolve<ICar>();

            Assert.AreNotSame(carFirst, carSecond);
        }

        [Test]
        public void Resolve_Transient_All_Generic()
        {
            const int EXPECTED_COUNT = 2;

            var container = Container.Create();

            container.RegisterAttribute<InjectAttribute>();
            container.Register<IColor>(typeof(ColorRed), typeof(ColorBlue));

            var colors = container.ResolveAll<IColor>();
            var actualCount = colors.Length;

            Assert.AreEqual(EXPECTED_COUNT, actualCount);
        }

        [Test]
        public void Resolve_Transient_All_Instances_Different()
        {
            const int EXPECTED_COUNT = 2;

            var container = Container.Create();

            var colorRed = new ColorRed();
            var colorBlue = new ColorBlue();

            container.RegisterAttribute<InjectAttribute>();

            container.Register<IColor>(Scope.Transient, colorRed);
            container.Register<IColor>(Scope.Transient, colorBlue);

            var colors = container.ResolveAll<IColor>();
            var actualCount = colors.Length;

            Assert.AreEqual(EXPECTED_COUNT, actualCount);
            Assert.AreNotEqual(colors[0], colors[1]);
        }

        [Test]
        public void Resolve_Transient_All_Instances_Same()
        {
            const int EXPECTED_COUNT = 2;

            var container = Container.Create();

            var colorRedOne = new ColorRed();
            var colorRedTwo = new ColorRed();

            container.RegisterAttribute<InjectAttribute>();

            container.Register(Scope.Transient, colorRedOne);
            container.Register(Scope.Transient, colorRedTwo);

            var colors = container.ResolveAll<ColorRed>();
            var actualCount = colors.Length;

            Assert.AreEqual(EXPECTED_COUNT, actualCount);
            Assert.AreNotEqual(colors[0], colors[1]);
        }

        [Test]
        public void Resolve_Transient_All_NonGeneric_Explicit()
        {
            const int EXPECTED_COUNT = 2;

            var container = Container.Create();

            container.RegisterAttribute<InjectAttribute>();
            container.Register<IColor>(typeof(ColorRed), typeof(ColorBlue));

            var colors = container.ResolveAll(typeof(IColor));
            var actualCount = colors.Length;

            Assert.AreEqual(EXPECTED_COUNT, actualCount);
        }

        [Test]
        public void Resolve_Transient_All_NonGeneric_Implicit()
        {
            const int EXPECTED_COUNT = 2;

            var container = Container.Create();

            container.RegisterAttribute<InjectAttribute>();
            container.Register<IColor>(typeof(ColorRed), typeof(ColorBlue));

            var colorsObject = container.Resolve(typeof(IColor[]));
            var colors = (object[])colorsObject;
            var actualCount = colors.Length;

            Assert.AreEqual(EXPECTED_COUNT, actualCount);
        }

        [Test]
        public void Resolve_Singleton_Contract()
        {
            var container = Container.Create();

            container.RegisterAttribute<InjectAttribute>();
            container.Register<ITimeMachine, TimeMachineDelorean>(Scope.Singleton);

            var machineFirst = container.Resolve<ITimeMachine>();
            var machineSecond = container.Resolve<ITimeMachine>();

            Assert.AreSame(machineFirst, machineSecond);
        }

        [Test]
        public void Resolve_Singleton_Result()
        {
            var container = Container.Create();

            container.RegisterAttribute<InjectAttribute>();
            container.Register<TimeMachineDelorean>(Scope.Singleton);

            var machineFirst = container.Resolve<TimeMachineDelorean>();
            var machineSecond = container.Resolve<TimeMachineDelorean>();

            Assert.AreSame(machineFirst, machineSecond);
        }

        [Test]
        public void Resolve_Singleton_Result_Throws_TypeNotRegisteredException()
        {
            var container = Container.Create();

            container.RegisterAttribute<InjectAttribute>();
            container.Register<ITimeMachine, TimeMachineDelorean>(Scope.Singleton);

            Assert.Throws<TypeNotRegisteredException>(() =>
            {
                container.Resolve<TimeMachineDelorean>();
            });
        }

        [Test]
        public void Resolve_Singleton_Constructor_Throws_TypeNotRegisteredException()
        {
            var container = Container.Create();

            container.RegisterAttribute<InjectAttribute>();

            container.Register<IAIPart, AIPartConsciousness>(Scope.Singleton);

            Assert.Throws<TypeNotRegisteredException>(() =>
            {
                container.Resolve<IAIPart>();
            });
        }

        [Test]
        public void Resolve_Installer_FromString()
        {
            var container = Container.Create();

            var installerName = typeof(InstallerDummy).FullName;
            var assembly = typeof(InstallerDummy).Assembly;

            container.RegisterAttribute<InjectAttribute>();

            container.Install(assembly, installerName);
        }

        [Test]
        public void Resolve_IntoRegistered_Singleton()
        {
            var container = Container.Create();

            var food = new CatFood();
            var petshop = new Petshop();

            container.RegisterAttribute<InjectAttribute>();

            container.Register<IPetFood>(Scope.Singleton, food);
            container.Register(Scope.Singleton, petshop);

            container.InjectIntoRegistered();

            var cached = container.GetCached<Petshop>();

            Assert.AreEqual(food, cached.Food);
        }

        [Test]
        public void Resolve_IntoRegistered_WithParameters_Singleton()
        {
            var container = Container.Create();

            var food = new CatFoodChips("Crunchy");
            var petshop = new Petshop();

            container.RegisterAttribute<InjectAttribute>();

            container.Register<IPetFood>(Scope.Singleton, food);
            container.Register(Scope.Singleton, petshop);

            container.InjectIntoRegistered();

            var cached = container.GetCached<Petshop>();

            Assert.AreEqual(food, cached.Food);
        }

        [Test]
        public void Resolve_IntoRegistered_Transient()
        {
            var container = Container.Create();

            var food = new CatFood();
            var petshopOne = new Petshop();
            var petshopTwo = new Petshop();

            container.RegisterAttribute<InjectAttribute>();

            container.Register<IPetFood>(Scope.Singleton, food);
            container.Register(Scope.Transient, petshopOne);
            container.Register(Scope.Transient, petshopTwo);

            container.InjectIntoRegistered();

            var cached = container.GetCachedMultiple<Petshop>();

            CollectionAssert.AllItemsAreNotNull(cached.Select(item => item.Food));
            CollectionAssert.AllItemsAreInstancesOfType(cached.Select(item => item.Food), typeof(IPetFood));
        }
    }
}