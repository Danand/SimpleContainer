﻿using SimpleContainer.Tests.DummyTypes;

using NUnit.Framework;

namespace SimpleContainer.Tests
{
    [TestFixture]
    public class InjectTests
    {
        [Test]
        public void Inject_Transient()
        {
            var container = Container.Create();

            container.Register<IEngine, EngineBig>(Scope.Transient);
            container.Register<ICar, CarFourWheelDrive>(Scope.Transient);

            var carFirst = container.Resolve<ICar>();
            var carSecond = container.Resolve<ICar>();

            Assert.AreNotSame(carFirst.Engine, carSecond.Engine);
        }

        [Test]
        public void Inject_Transient_All_Inline()
        {
            const int EXPECTED_COUNT = 2;

            var container = Container.Create();

            container.Register<IColor>(typeof(ColorRed), typeof(ColorBlue));
            container.Register<IColorPalette, ColorPalette>(Scope.Singleton);

            var palette = container.Resolve<IColorPalette>();
            var actualCount = palette.Colors.Length;

            Assert.AreEqual(EXPECTED_COUNT, actualCount);
        }

        [Test]
        public void Inject_Transient_All_Multiline()
        {
            const int EXPECTED_COUNT = 2;

            var container = Container.Create();

            container.Register<IColor, ColorRed>(Scope.Transient);
            container.Register<IColor, ColorBlue>(Scope.Transient);
            container.Register<IColorPalette, ColorPalette>(Scope.Singleton);

            var palette = container.Resolve<IColorPalette>();
            var actualCount = palette.Colors.Length;

            Assert.AreEqual(EXPECTED_COUNT, actualCount);
        }

        [Test]
        public void Inject_Singleton()
        {
            var container = Container.Create();

            container.Register<IPhysics, PhysicsPlanetEarth>(Scope.Singleton);
            container.Register<IEngine, EngineMedium>(Scope.Transient);

            var enigneFirst = container.Resolve<IEngine>();
            var enigneSecond = container.Resolve<IEngine>();

            Assert.AreSame(enigneFirst.Physics, enigneSecond.Physics);
        }

        [Test]
        public void Inject_Factory()
        {
            var container = Container.Create();

            container.Register<IEngine, EngineBig>(Scope.Transient);
            container.Register<ICar, CarFourWheelDrive>(Scope.Transient);

            container.RegisterFactory<CarFactory>();

            var factory = container.Resolve<CarFactory>();
            var car = factory.Create<CarFourWheelDrive>();

            Assert.IsInstanceOf<EngineBig>(car.Engine);
        }

        [Test]
        public void Inject_IntoField_Public()
        {
            var container = Container.Create();

            container.Register<IPhysics, PhysicsPlanetEarth>(Scope.Singleton);
            container.Register<IEngine, EngineMedium>(Scope.Transient);

            var expected = container.Resolve<IPhysics>();
            var engine = container.Resolve<IEngine>();

            Assert.AreSame(expected, engine.PhysicsFromFieldPublic);
        }

        [Test]
        public void Inject_IntoField_Private()
        {
            var container = Container.Create();

            container.Register<IPhysics, PhysicsPlanetEarth>(Scope.Singleton);
            container.Register<IEngine, EngineMedium>(Scope.Transient);

            var expected = container.Resolve<IPhysics>();
            var engine = container.Resolve<IEngine>();

            Assert.AreSame(expected, engine.PhysicsFromFieldPrivate);
        }

        [Test]
        public void Inject_IntoProperty_Public()
        {
            var container = Container.Create();

            container.Register<IPhysics, PhysicsPlanetEarth>(Scope.Singleton);
            container.Register<IEngine, EngineMedium>(Scope.Transient);

            var expected = container.Resolve<IPhysics>();
            var engine = container.Resolve<IEngine>();

            Assert.AreSame(expected, engine.PhysicsFromPropertyPublic);
        }

        [Test]
        public void Inject_IntoProperty_Private()
        {
            var container = Container.Create();

            container.Register<IPhysics, PhysicsPlanetEarth>(Scope.Singleton);
            container.Register<IEngine, EngineMedium>(Scope.Transient);

            var expected = container.Resolve<IPhysics>();
            var engine = container.Resolve<IEngine>();

            Assert.AreSame(expected, engine.PhysicsFromPropertyPrivate);
        }
    }
}