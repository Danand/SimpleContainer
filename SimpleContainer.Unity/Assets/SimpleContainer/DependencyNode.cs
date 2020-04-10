using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SimpleContainer
{
    internal sealed class DependencyNode
    {
        private const int CYCLES_MAX = 300;

        public Type ContractType { get; set; }

        public Type ResultType { get; set; }

        public object Instance { get; set; }

        public HashSet<object> CachedInstances { get; } = new HashSet<object>();

        public Scope Scope { get; set; }

        public ConstructorInfo Constructor { get; set; }

        public DependencyDictionary ConstructorDependencies { get; set; }

        public Dictionary<PropertyInfo, DependencyLink> PropertyDependencies { get; set; }

        public Dictionary<FieldInfo, DependencyLink> FieldDependencies { get; set; }

        public Dictionary<MethodInfo, DependencyDictionary> MethodDependencies { get; set; }

        public IEnumerable<DependencyLink> GetAllDependencies()
        {
            var cycleCounter = 0;

            foreach (var link in IterateSiblingLinks(ConstructorDependencies))
            {
                yield return Iteration(link, ref cycleCounter);
            }

            foreach (var link in IterateSiblingLinks(PropertyDependencies.Values))
            {
                yield return Iteration(link, ref cycleCounter);
            }

            foreach (var link in IterateSiblingLinks(FieldDependencies.Values))
            {
                yield return Iteration(link, ref cycleCounter);
            }

            foreach (var link in IterateSiblingLinks(MethodDependencies.Values.SelectMany(link => link)))
            {
                yield return Iteration(link, ref cycleCounter);
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

        private DependencyLink Iteration(DependencyLink link, ref int cycleCounter)
        {
            cycleCounter++;

            if (cycleCounter > CYCLES_MAX)
                throw new Exception($"{nameof(cycleCounter)} > {CYCLES_MAX}");

            return link;
        }
    }
}