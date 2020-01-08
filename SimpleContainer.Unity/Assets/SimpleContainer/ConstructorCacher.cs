using System;
using System.Collections.Generic;
using System.Reflection;

using SimpleContainer.Interfaces;

namespace SimpleContainer
{
    public sealed class ConstructorCacher : IConstructorCacher
    {
        private readonly Dictionary<Type, ConstructorInfo> cachedConstructors = new Dictionary<Type, ConstructorInfo>();

        ConstructorInfo IConstructorCacher.GetConstructor(Type type)
        {
            if (cachedConstructors.TryGetValue(type, out var suitableConstructor))
                return suitableConstructor;

            var constructors = type.GetConstructors();
            suitableConstructor = constructors[0];

            cachedConstructors.Add(type, suitableConstructor);

            return suitableConstructor;
        }
    }
}