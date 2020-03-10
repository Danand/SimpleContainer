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

        public IEnumerable<DependencyLink> GetAllDependencies()
        {
            foreach (var link in IterateSiblingLinks(ConstructorDependencies))
            {
                yield return link;
            }

            foreach (var link in IterateSiblingLinks(PropertyDependencies.Values))
            {
                yield return link;
            }

            foreach (var link in IterateSiblingLinks(FieldDependencies.Values))
            {
                yield return link;
            }

            foreach (var link in IterateSiblingLinks(MethodDependencies.Values.SelectMany(link => link)))
            {
                yield return link;
            }
        }

        private IEnumerable<DependencyLink> IterateSiblingLinks(IEnumerable<DependencyLink> links)
        {
            foreach (var link in links)
            {
                var currentLink = link;

                yield return currentLink;

                currentLink = currentLink.NextLink;

                while (currentLink != null)
                {
                    yield return currentLink;
                    currentLink = currentLink.NextLink;
                }
            }
        }
    }
}