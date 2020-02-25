using System;
using System.Collections.Generic;

namespace SimpleContainer
{
    public sealed class DependencyGraph
    {
        public IList<DependencyNode> RootNodes { get; set; }

        public void Register<TContract, TResult>(Scope scope, TResult instance)
        {
            RootNodes.Add(new DependencyNode
            {
                ContractType = typeof(TContract),
                ResultType = typeof(TResult),
                Scope = scope,
                Instance = instance
            });
        }

        public void Link()
        {
            foreach (var rootNode in RootNodes)
            {
                PopulateChildNodes(rootNode);
            }

            foreach (var rootNode in RootNodes)
            {
                LinkDependenciesRecursive(rootNode, RootNodes);
            }
        }

        public void Resolve()
        {
            throw new NotImplementedException();
        }

        private void PopulateChildNodes(DependencyNode node)
        {
            throw new NotImplementedException();
        }

        private void LinkDependenciesRecursive(DependencyNode node, IList<DependencyNode> rootNodes)
        {
            throw new NotImplementedException();
        }
    }
}