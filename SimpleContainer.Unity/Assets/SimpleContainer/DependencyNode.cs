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

        public IEnumerable<DependencyLink> AllDependencies
        {
            get
            {
                foreach (var link in ConstructorDependencies)
                {
                    yield return link;
                }

                foreach (var link in PropertyDependencies.Values)
                {
                    yield return link;
                }

                foreach (var link in FieldDependencies.Values)
                {
                    yield return link;
                }

                foreach (var link in MethodDependencies.Values.SelectMany(link => link))
                {
                    yield return link;
                }
            }
        }
    }
}