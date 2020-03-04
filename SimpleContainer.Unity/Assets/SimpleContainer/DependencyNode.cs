using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SimpleContainer
{
    public sealed class DependencyNode
    {
        public Type ContractType { get; set; }

        public Type ResultType { get; set; }

        public object Instance { get; set; }

        public Scope Scope { get; set; }

        public ConstructorInfo Constructor { get; set; }

        public DependencyDictionary ConstructorDependencies { get; set; }

        public Dictionary<PropertyInfo, DependencyLink> PropertyDependencies { get; set; }

        public Dictionary<FieldInfo, DependencyLink> FieldDependencies { get; set; }

        public Dictionary<MethodInfo, DependencyDictionary> MethodDependencies { get; set; }
    }
}