using System;
using System.Collections.Generic;
using System.Linq;

using SimpleContainer.Exceptions;

namespace SimpleContainer
{
    internal sealed class DependencyRegistry
    {
        private readonly IList<DependencyNode> rootNodes;

        private readonly HashSet<Type> contractTypes = new HashSet<Type>();

        public DependencyRegistry(IList<DependencyNode> rootNodes)
        {
            this.rootNodes = rootNodes;
        }

        public void ThrowIfRepeating(DependencyLink dependency)
        {
            if (!contractTypes.Add(dependency.ContractType))
            {
                var circularNode = rootNodes.First(node => node.ContractType == dependency.ContractType);
                throw new CircularDependencyException(dependency.ContractType, BindingsPrinter.GetBindingsString(rootNodes, circularNode: circularNode));
            }
        }

        public void Remove(DependencyLink dependency)
        {
            contractTypes.RemoveWhere(type => type == dependency.ContractType);
        }
    }
}