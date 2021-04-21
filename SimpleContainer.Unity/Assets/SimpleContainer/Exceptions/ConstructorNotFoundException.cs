using System;
using System.Linq;
using System.Reflection;

namespace SimpleContainer.Exceptions
{
    public sealed class ConstructorNotFoundException : Exception
    {
        private enum AccessModifier
        {
            Private,
            Protected,
            Internal,
            Public,
            ProtectedInternal
        }

        public ConstructorNotFoundException(Type type) : base($"Result type '{type.Name}' does not contain any public instance constructors!" +
                                                              $"{Environment.NewLine}" +
                                                              "Other constructors:" +
                                                              $"{Environment.NewLine}" +
                                                              $"{FormatConstructors(type)}") { }

        private static string FormatConstructors(Type type)
        {
            return string.Join(
                Environment.NewLine,
                type.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)
                    .Select(constructor => $"{GetAccessModifier(constructor)} {constructor}"));
        }

        private static AccessModifier GetAccessModifier(ConstructorInfo methodInfo)
        {
            if (methodInfo.IsPrivate)
                return AccessModifier.Private;

            if (methodInfo.IsFamily)
                return AccessModifier.Protected;

            if (methodInfo.IsFamilyOrAssembly)
                return AccessModifier.ProtectedInternal;

            if (methodInfo.IsAssembly)
                return AccessModifier.Internal;

            if (methodInfo.IsPublic)
                return AccessModifier.Public;

            throw new ArgumentException("Did not find access modifier", "methodInfo");
        }
    }
}