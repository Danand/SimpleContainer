using System;
using System.Collections.Generic;

namespace SimpleContainer
{
    public sealed class DependencyDictionary
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

        public void Add(Type type)
        {
            links.Add(DependencyLink.Create(type, null));
        }
    }
}