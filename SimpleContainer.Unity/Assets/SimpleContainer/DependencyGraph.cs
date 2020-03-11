using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using SimpleContainer.Exceptions;
using SimpleContainer.Extensions;
using SimpleContainer.Utils;

namespace SimpleContainer
{
    public sealed class DependencyGraph
    {
        private readonly Container container;
        private readonly HashSet<object> injectedIntoInstances = new HashSet<object>();
        private readonly HashSet<int> injectedIntoMembers = new HashSet<int>();

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

            foreach (var rootNode in RootNodes)
            {
                ThrowIfCircularDependency(rootNode);
            }
        }

        public object Resolve(Type keyType)
        {
            var contractType = keyType.HasElementType ? keyType.GetElementType() : keyType;
            var foundNodes = RootNodes.Where(node => node.ContractType == contractType).ToArray();

            if (foundNodes.Length == 0)
                throw new TypeNotRegisteredException(contractType, GetBindingsString());

            foreach (var foundNode in foundNodes)
            {
                if (foundNode.Scope == Scope.Transient || foundNode.Instance == null)
                    foundNode.Instance = Instantiate(foundNode);

                InjectIntoInstance(foundNode);
            }

            if (keyType.HasElementType)
            {
                var instances = foundNodes.Select(node => ReflectionUtils.CastNonGeneric(node.Instance, contractType)).ToArray();
                return ReflectionUtils.CastNonGenericArray(contractType, instances);
            }

            return foundNodes[0].Instance;
        }

        internal string GetBindingsString()
        {
            return "WIP";
        }

        internal void InjectIntoInstance(DependencyNode node)
        {
            if (injectedIntoInstances.Add(node.Instance))
            {
                InjectIntoProperties(node);
                InjectIntoFields(node);
                InjectIntoMethods(node);
            }
        }

        private void LinkDependencies(DependencyNode node, IList<DependencyNode> rootNodes)
        {
            var links = node.GetAllDependencies().ToArray();

            foreach (var link in links)
            {
                var foundNodes = rootNodes.Where(rootNode => rootNode.ContractType == link.ContractType).ToArray();

                if (foundNodes.Length == 0)
                    throw new TypeNotRegisteredException(link.ContractType, GetBindingsString());

                DependencyLink currentLink = link;

                for (var i = 0; i < foundNodes.Length; i++)
                {
                    var foundNode = foundNodes[i];

                    currentLink.Node = foundNode;

                    if (foundNodes.Length > i + 1)
                    {
                        currentLink.NextLink = currentLink.NextLink ?? DependencyLink.Create(link.KeyType);
                    }

                    currentLink = currentLink.NextLink;
                }
            }
        }

        private void CollectDependencies(DependencyNode node)
        {
            node.Constructor = node.ResultType.GetConstructors()[0];
            node.ConstructorDependencies = GetConstructorDependencies(node);
            node.PropertyDependencies = GetPropertyDependencies(node);
            node.FieldDependencies = GetFieldDependencies(node);
            node.MethodDependencies = GetMethodDependencies(node);
        }

        private object Instantiate(DependencyNode node)
        {
            var args = node.ConstructorDependencies.Select(link => Resolve(link.KeyType)).ToArray();
            return node.Constructor.Invoke(args);
        }

        private void ThrowIfCircularDependency(DependencyNode node)
        {
            var hasCircularDependency = node.GetAllDependencies().Flatten(link => link.Node.GetAllDependencies())
                                            .Any(link => link.ContractType == node.ContractType);

            if (hasCircularDependency)
                throw new CircularDependencyException(node.ContractType, GetBindingsString());
        }

        private void InjectIntoProperties(DependencyNode node)
        {
            foreach (var pair in node.PropertyDependencies)
            {
                if (CheckNeedsInjectIntoMember(pair.Key, node.Instance))
                {
                    var value = Resolve(pair.Value.KeyType);
                    pair.Key.SetValue(node.Instance, value);
                    MarkMemberInjectedInto(pair.Key, node.Instance);
                }
            }
        }

        private void InjectIntoFields(DependencyNode node)
        {
            foreach (var pair in node.FieldDependencies)
            {
                if (CheckNeedsInjectIntoMember(pair.Key, node.Instance))
                {
                    var value = Resolve(pair.Value.KeyType);
                    pair.Key.SetValue(node.Instance, value);
                    MarkMemberInjectedInto(pair.Key, node.Instance);
                }
            }
        }

        private void InjectIntoMethods(DependencyNode node)
        {
            foreach (var pair in node.MethodDependencies)
            {
                if (CheckNeedsInjectIntoMember(pair.Key, node.Instance))
                {
                    var args = pair.Value.Select(link => Resolve(link.KeyType)).ToArray();
                    pair.Key.Invoke(node.Instance, args);
                    MarkMemberInjectedInto(pair.Key, node.Instance);
                }
            }
        }

        private bool CheckNeedsInjectIntoMember(MemberInfo member, object instance)
        {
            return !injectedIntoMembers.Contains(member.GetHashCode() + instance.GetHashCode());
        }

        private void MarkMemberInjectedInto(MemberInfo member, object instance)
        {
            injectedIntoMembers.Add(member.GetHashCode() + instance.GetHashCode());
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
                result.Add(property, DependencyLink.Create(property.PropertyType));
            }

            return result;
        }

        private Dictionary<FieldInfo, DependencyLink> GetFieldDependencies(DependencyNode node)
        {
            var result = new Dictionary<FieldInfo, DependencyLink>();
            var fields = node.ResultType.GetFields().Where(field => container.injectAttributeTypes.Any(attributeType => field.GetCustomAttribute(attributeType) != null));

            foreach (var field in fields)
            {
                result.Add(field, DependencyLink.Create(field.FieldType));
            }

            return result;
        }

        private Dictionary<MethodInfo, DependencyDictionary> GetMethodDependencies(DependencyNode node)
        {
            var result = new Dictionary<MethodInfo, DependencyDictionary>();
            var methods = node.ResultType.GetMethods().Where(field => container.injectAttributeTypes.Any(attributeType => field.GetCustomAttribute(attributeType) != null));

            foreach (var method in methods)
            {
                var parameters = method.GetParameters();

                if (parameters.Length == 0)
                    continue;

                DependencyDictionary dependencyDictionary = new DependencyDictionary();

                result.Add(method, dependencyDictionary);

                foreach (var parameter in parameters)
                {
                    dependencyDictionary.Add(parameter.ParameterType);
                }
            }

            return result;
        }
    }
}