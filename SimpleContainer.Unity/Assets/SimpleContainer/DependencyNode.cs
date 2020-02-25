using System;
using System.Collections.Generic;

namespace SimpleContainer
{
    public sealed class DependencyNode
    {
        public Scope Scope { get; set; }

        public Type ContractType { get; set; }

        public Type ResultType { get; set; }

        public object Instance { get; set; }

        public IList<DependencyNode> ChildNodes { get; } = new List<DependencyNode>();
    }
}