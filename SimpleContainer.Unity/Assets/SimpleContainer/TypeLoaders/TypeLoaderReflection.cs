using System;
using System.Linq;

using SimpleContainer.Interfaces;

namespace SimpleContainer.TypeLoaders
{
    internal sealed class TypeLoaderReflection : ITypeLoader
    {
        /// <summary>
        /// Finds <see cref="Type"/> by <see cref="Type.Name"/>
        /// via searching in every loaded assembly.
        /// </summary>
        Type ITypeLoader.Load(string typeName)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var types = assemblies.SelectMany(assembly => assembly.GetTypes());
            var foundType = types.FirstOrDefault(type => type.Name == typeName);

            if (foundType == null)
                throw new TypeLoadException($"Type '{typeName}' not found!");

            return foundType;
        }
    }
}