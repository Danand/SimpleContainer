using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
            node.Constructor = node.ResultType.GetConstructors()[0];
            node.ConstructorDependencies = GetConstructorDependencies(node);
            node.PropertyDependencies = GetPropertyDependencies(node);
            node.FieldDependencies = GetFieldDependencies(node);
            node.MethodDependencies = GetMethodDependencies(node);
        }

        private void LinkDependencies(DependencyNode node, IList<DependencyNode> rootNodes)
        {
            
        }

        private DependencyDictionary GetConstructorDependencies(DependencyNode node)
        {
            var result = new DependencyDictionary();
            var constructors = node.ResultType.GetConstructors();

            foreach (var constructor in constructors)
            {
                var parameters = constructor.GetParameters();

                if (parameters.Length == 0)
                    continue;

                foreach (var parameter in parameters)
                {
                    result.Add(parameter.ParameterType);
                }
            }

            return result;
        }

        private Dictionary<PropertyInfo, DependencyLink> GetPropertyDependencies(DependencyNode node)
        {
            var result = new Dictionary<PropertyInfo, DependencyLink>();
            var properties = node.ResultType.GetProperties().Where(property => container.injectAttributeTypes.Any(attributeType => property.GetCustomAttribute(attributeType) != null));

            foreach (var property in properties)
            {
                var type = property.PropertyType.HasElementType
                               ? property.PropertyType.GetElementType()
                               : property.PropertyType;

                result.Add(property, DependencyLink.Create(type));
            }

            return result;
        }

        private Dictionary<FieldInfo, Dictionary<Type, DependencyNode>> GetFieldDependencies(DependencyNode node)
        {
            var result = new Dictionary<FieldInfo, Dictionary<Type, DependencyNode>>();
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

        private Dictionary<MethodInfo, Dictionary<Type, DependencyNode>> GetMethodDependencies(DependencyNode node)
        {
            var result = new Dictionary<MethodInfo, Dictionary<Type, DependencyNode>>();
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