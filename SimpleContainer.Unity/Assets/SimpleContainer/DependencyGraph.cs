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
            // TODO: link element type dependencies.
            foreach (var dependency in node.GetAllDependencies())
            {
                var foundResolution = rootNodes.FirstOrDefault(rootNode => rootNode.ContractType == dependency.ContractType);

                if (foundResolution == null)
                    throw new TypeNotRegisteredException(dependency.ContractType, GetBindingsString());

                dependency.ResultType = foundResolution.ResultType;
                dependency.Scope = foundResolution.Scope;
                dependency.Instance = foundResolution.Instance;
            }
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
                        ContractType = type
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
                    ContractType = type
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
                    ContractType = type
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
                        ContractType = type
                    });
                }
            }

            return result;
        }
    }
}