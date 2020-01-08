using System;
using System.Reflection;

using SimpleContainer.Interfaces;

namespace SimpleContainer.Activators
{
    internal class ActivatorReflection : IActivator
    {
        private readonly IConstructorCacher constructorCacher;

        public ActivatorReflection(IConstructorCacher constructorCacher)
        {
            this.constructorCacher = constructorCacher;
        }

        object IActivator.CreateInstance(Type type)
        {
            var constructor = constructorCacher.GetConstructor(type);
            return constructor.Invoke(new object[0]);
        }

        object IActivator.CreateInstance(Type type, object[] args)
        {
            var constructor = constructorCacher.GetConstructor(type);
            return constructor.Invoke(args);
        }

        object IActivator.CreateInstance(ConstructorInfo constructor, object[] args)
        {
            return constructor.Invoke(args);
        }
    }
}
