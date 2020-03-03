using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SimpleContainer
{
    public sealed class DependencyNode
    {
        public Scope Scope { get; set; }

        public Type ContractType { get; set; }

        public Type ResultType { get; set; }

        public object Instance { get; set; }

        public Dictionary<ConstructorInfo, IList<DependencyNode>> ConstructorDependencies { get; set; }

        public Dictionary<PropertyInfo, IList<DependencyNode>> PropertyDependencies { get; set; }

        public Dictionary<FieldInfo, IList<DependencyNode>> FieldDependencies { get; set; }

        public Dictionary<MethodInfo, IList<DependencyNode>> MethodDependencies { get; set; }

        public IEnumerable<DependencyNode> GetAllDependencies()
        {
            return ConstructorDependencies.Values.SelectMany(dep => dep)
                                                 .Concat(PropertyDependencies.Values.SelectMany(dep => dep))
                                                 .Concat(FieldDependencies.Values.SelectMany(dep => dep))
                                                 .Concat(MethodDependencies.Values.SelectMany(dep => dep));
        }
    }
}