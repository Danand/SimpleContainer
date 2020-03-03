using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using SimpleContainer.Exceptions;

namespace SimpleContainer
{
    public sealed class DependencyGraph
    {
        private readonly Container container;

        public DependencyGraph(Container container)
        {
            this.container = container;
        }

        public IList<DependencyNode> RootNodes { get; } = new List<DependencyNode>();

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
                CollectDependencies(rootNode);
            }

            foreach (var rootNode in RootNodes)
            {
                LinkDependencies(rootNode, RootNodes);
            }
        }

        public void Resolve()
        {
            throw new NotImplementedException();
        }

        public string GetBindingsString()
        {
            return "WIP";
        }

        private void CollectDependencies(DependencyNode node)
        {
            node.ConstructorDependencies = GetConstructorDependencies(node);
            node.PropertyDependencies = GetPropertyDependencies(node);
            node.FieldDependencies = GetFieldDependencies(node);
            node.MethodDependencies = GetMethodDependencies(node);
        }

        private void LinkDependencies(DependencyNode node, IList<DependencyNode> rootNodes)
        {
            foreach (var dependency in node.GetAllDependencies().ToArray())
            {
                var foundResolvings = rootNodes.Where(rootNode => rootNode.ContractType == dependency.ContractType).ToArray();

                if (foundResolvings.Length == 0)
                    throw new TypeNotRegisteredException(dependency.ContractType, GetBindingsString());

                dependency.ResultType = foundResolvings[0].ResultType;
                dependency.Scope = foundResolvings[0].Scope;
                dependency.Instance = foundResolvings[0].Instance;

                if (dependency.KeyType.HasElementType)
                {
                    for (var i = 1; i < foundResolvings.Length; i++)
                    {
                        var resolving = foundResolvings[i];

                        AddSibling(dependency, new DependencyNode
                        {
                            ContractType = resolving.ContractType,
                            ResultType = resolving.ResultType,
                            KeyType = resolving.ContractType,
                            Instance = resolving.Instance
                        });
                    }
                }
            }
        }

        public void AddSibling(DependencyNode node, DependencyNode sibling)
        {
            sibling.Siblings = node.Siblings;
            node.Siblings.Add(sibling);
        }

        private Dictionary<ConstructorInfo, IList<DependencyNode>> GetConstructorDependencies(DependencyNode node)
        {
            var result = new Dictionary<ConstructorInfo, IList<DependencyNode>>();
            var constructors = node.ResultType.GetConstructors();

            foreach (var constructor in constructors)
            {
                var parameters = constructor.GetParameters();

                if (parameters.Length == 0)
                    continue;

                var dependencies = new List<DependencyNode>();

                result.Add(constructor, dependencies);

                foreach (var parameter in parameters)
                {
                    var type = parameter.ParameterType.HasElementType
                                   ? parameter.ParameterType.GetElementType()
                                   : parameter.ParameterType;

                    dependencies.Add(new DependencyNode
                    {
                        ContractType = type,
                        KeyType = parameter.ParameterType,
                        Siblings = dependencies
                    });
                }
            }

            return result;
        }

        private Dictionary<PropertyInfo, IList<DependencyNode>> GetPropertyDependencies(DependencyNode node)
        {
            var result = new Dictionary<PropertyInfo, IList<DependencyNode>>();
            var properties = node.ResultType.GetProperties().Where(property => container.injectAttributeTypes.Any(attributeType => property.GetCustomAttribute(attributeType) != null));

            foreach (var property in properties)
            {
                var dependencies = new List<DependencyNode>();

                result.Add(property, dependencies);

                var type = property.PropertyType.HasElementType
                               ? property.PropertyType.GetElementType()
                               : property.PropertyType;

                dependencies.Add(new DependencyNode
                {
                    ContractType = type,
                    KeyType = property.PropertyType,
                    Siblings = dependencies
                });
            }

            return result;
        }

        private Dictionary<FieldInfo, IList<DependencyNode>> GetFieldDependencies(DependencyNode node)
        {
            var result = new Dictionary<FieldInfo, IList<DependencyNode>>();
            var fields = node.ResultType.GetFields().Where(field => container.injectAttributeTypes.Any(attributeType => field.GetCustomAttribute(attributeType) != null));

            foreach (var field in fields)
            {
                var dependencies = new List<DependencyNode>();

                result.Add(field, dependencies);

                var type = field.FieldType.HasElementType
                               ? field.FieldType.GetElementType()
                               : field.FieldType;

                dependencies.Add(new DependencyNode
                {
                    ContractType = type,
                    KeyType = field.FieldType,
                    Siblings = dependencies
                });
            }

            return result;
        }

        private Dictionary<MethodInfo, IList<DependencyNode>> GetMethodDependencies(DependencyNode node)
        {
            var result = new Dictionary<MethodInfo, IList<DependencyNode>>();
            var methods = node.ResultType.GetMethods().Where(field => container.injectAttributeTypes.Any(attributeType => field.GetCustomAttribute(attributeType) != null));

            foreach (var method in methods)
            {
                var parameters = method.GetParameters();

                if (parameters.Length == 0)
                    continue;

                var dependencies = new List<DependencyNode>();

                result.Add(method, dependencies);

                foreach (var parameter in parameters)
                {
                    var type = parameter.ParameterType.HasElementType
                                   ? parameter.ParameterType.GetElementType()
                                   : parameter.ParameterType;

                    dependencies.Add(new DependencyNode
                    {
                        ContractType = type,
                        KeyType = parameter.ParameterType,
                        Siblings = dependencies
                    });
                }
            }

            return result;
        }
    }
}