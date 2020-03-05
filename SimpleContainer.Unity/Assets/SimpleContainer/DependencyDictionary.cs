using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SimpleContainer
{
    public sealed class DependencyDictionary : IEnumerable<DependencyLink>
    {
        private readonly Dictionary<Type, DependencyLink> links = new Dictionary<Type, DependencyLink>();

        public DependencyNode this[Type key]
        {
            get
            {
                if (links.TryGetValue(key, out var link))
                    return link.Node;

                return null;
            }
            set
            {
                if (links.TryGetValue(key, out var link))
                {
                    link.Node = value;
                }

                link = DependencyLink.Create(key, value);

                links.Add(key, link);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<DependencyLink> GetEnumerator()
        {
            foreach (var link in links.Values)
            {
                yield return link;
            }
        }

        public void Add(Type type)
        {
            links.Add(type, DependencyLink.Create(type, null));
        }

        public void Add(Type type, DependencyNode node)
        {
            links.Add(type, DependencyLink.Create(type, node));
        }
    }
}