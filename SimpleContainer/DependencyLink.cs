using System;

namespace SimpleContainer
{
    internal sealed class DependencyLink
    {
        public Type KeyType { get; set; }

        public Type ContractType { get; set; }

        public DependencyNode Node { get; set; }

        public DependencyLink NextLink { get; set; }

        public static DependencyLink Create(Type type)
        {
            return Create(type, null);
        }

        public static DependencyLink Create(Type type, DependencyNode node)
        {
            return new DependencyLink
            {
                KeyType = type,
                ContractType = type.HasElementType ? type.GetElementType() : type,
                Node = node
            };
        }
    }
}