﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using SimpleContainer.Exceptions;
using SimpleContainer.Utils;

namespace SimpleContainer
{
    internal sealed class DependencyManager
    {
        private readonly Container container;
        private readonly HashSet<object> injectedIntoInstances = new HashSet<object>();
        private readonly HashSet<int> injectedIntoMembers = new HashSet<int>();

        private bool isLinked;

        public DependencyManager(Container container)
        {
            this.container = container;
        }

        public IList<DependencyNode> RootNodes { get; } = new List<DependencyNode>();

        public void Register<TContract, TResult>(Scope scope, TResult instance)
            where TResult : TContract
        {
            MarkNotLinked();

            var node = new DependencyNode
            {
                ContractType = typeof(TContract),
                ResultType = typeof(TResult),
                Scope = scope,
                Instance = instance,
                IsPreInstantiated = instance != null
            };

            if (instance != null)
                node.CachedInstances.Add(instance);

            RootNodes.Add(node);
        }

        public void Register(Type contractType, Type resultType, Scope scope, object instance)
        {
            ThrowIfWrongTypes(contractType, resultType);

            MarkNotLinked();

            var node = new DependencyNode
            {
                ContractType = contractType,
                ResultType = resultType,
                Scope = scope,
                Instance = instance,
                IsPreInstantiated = instance != null
            };

            if (instance != null)
                node.CachedInstances.Add(instance);

            RootNodes.Add(node);
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

            isLinked = true;
        }

        public object Resolve(Type keyType)
        {
            LinkIfNeeded();

            var contractType = keyType.HasElementType ? keyType.GetElementType() : keyType;
            var foundNodes = RootNodes.Where(node => node.ContractType == contractType).ToArray();

            if (foundNodes.Length == 0)
                throw new TypeNotRegisteredException(contractType, BindingsPrinter.GetBindingsString(RootNodes));

            foreach (var foundNode in foundNodes)
            {
                if (foundNode.Scope == Scope.Transient || foundNode.Instance == null)
                {
                    foundNode.Instance = Instantiate(foundNode);
                }

                foundNode.CachedInstances.Add(foundNode.Instance);

                InjectIntoInstance(foundNode);
            }

            if (keyType.HasElementType)
            {
                var instances = foundNodes.Select(node => ReflectionUtils.CastNonGeneric(node.Instance, contractType)).ToArray();
                return ReflectionUtils.CastNonGenericArray(contractType, instances);
            }

            return foundNodes[0].Instance;
        }

        public object GetCachedInstance(Type contractType)
        {
            return RootNodes.First(node => node.ContractType == contractType).CachedInstances.First();
        }

        public object[] GetCachedInstances(Type contractType)
        {
            return RootNodes.Where(node => node.ContractType == contractType)
                            .SelectMany(node => node.CachedInstances)
                            .ToArray();
        }

        public void InjectIntoRegistered()
        {
            foreach (var rootNode in RootNodes)
            {
                foreach (object cachedInstance in rootNode.CachedInstances)
                {
                    InjectIntoInstance(cachedInstance);
                }
            }
        }

        public void InjectIntoInstance(object instance)
        {
            var foundNodes = RootNodes.Where(node => node.ResultType == instance.GetType()).ToArray();

            foreach (var foundNode in foundNodes)
            {
                InjectIntoInstance(foundNode);
            }
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
                    throw new TypeNotRegisteredException(link.ContractType, BindingsPrinter.GetBindingsString(RootNodes, notRegisteredType: link.ContractType));

                AssignNodesToLinks(link, foundNodes);
            }
        }

        private void AssignNodesToLinks(DependencyLink link, DependencyNode[] foundNodes)
        {
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

        private void LinkIfNeeded()
        {
            if (!isLinked)
                Link();
        }

        private void MarkNotLinked()
        {
            isLinked = false;
        }

        private void CollectDependencies(DependencyNode node)
        {
            node.Constructor = GetConstructor(node);

            if (!node.IsPreInstantiated)
            {
                node.ConstructorDependencies = GetConstructorDependencies(node);
            }

            node.PropertyDependencies = GetPropertyDependencies(node);
            node.FieldDependencies = GetFieldDependencies(node);
            node.MethodDependencies = GetMethodDependencies(node);
        }

        private ConstructorInfo GetConstructor(DependencyNode node)
        {
            var constructors = node.ResultType.GetConstructors();

            if (constructors.Length == 0)
            {
                throw new ConstructorNotFoundException(node.ResultType);
            }

            return constructors[0];
        }

        private object Instantiate(DependencyNode node)
        {
            if (node.ConstructorDependencies == null)
            {
                return container.InternalDependencies.Activator.CreateInstance(node.Constructor, new object[0]);
            }

            var args = node.ConstructorDependencies.Select(link => Resolve(link.KeyType)).ToArray();

            if (container.InternalDependencies == null)
                return node.Constructor.Invoke(args);

            return container.InternalDependencies.Activator.CreateInstance(node.Constructor, args);
        }

        private void ThrowIfCircularDependency(DependencyNode node)
        {
            var dependencyCounter = new DependencyRegistry(RootNodes);
            _ = FlattenDependencies(node.GetAllDependencies(), link => link.Node.GetAllDependencies(), dependencyCounter).ToArray();
        }

        private IEnumerable<DependencyLink> FlattenDependencies(
            IEnumerable<DependencyLink>                         dependencies,
            Func<DependencyLink, IEnumerable<DependencyLink>>   selector,
            DependencyRegistry                                  dependencyRegistry)
        {
            foreach (DependencyLink dependency in dependencies)
            {
                dependencyRegistry.ThrowIfRepeating(dependency);

                yield return dependency;

                var children = selector.Invoke(dependency);

                foreach (DependencyLink child in FlattenDependencies(children, selector, dependencyRegistry))
                {
                    yield return child;
                }

                dependencyRegistry.Remove(dependency);
            }
        }

        private void ThrowIfWrongTypes(Type contractType, Type resultType)
        {
            if (!contractType.IsAssignableFrom(resultType))
                throw new WrongTypesException(contractType, resultType);
        }

        private void InjectIntoProperties(DependencyNode node)
        {
            LinkIfNeeded();

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
            var properties = node.ResultType.GetProperties().Where(property => container.InjectAttributeTypes.Any(attributeType => property.GetCustomAttribute(attributeType) != null));

            foreach (var property in properties)
            {
                result.Add(property, DependencyLink.Create(property.PropertyType));
            }

            return result;
        }

        private Dictionary<FieldInfo, DependencyLink> GetFieldDependencies(DependencyNode node)
        {
            var result = new Dictionary<FieldInfo, DependencyLink>();
            var fields = node.ResultType.GetFields().Where(field => container.InjectAttributeTypes.Any(attributeType => field.GetCustomAttribute(attributeType) != null));

            foreach (var field in fields)
            {
                result.Add(field, DependencyLink.Create(field.FieldType));
            }

            return result;
        }

        private Dictionary<MethodInfo, DependencyDictionary> GetMethodDependencies(DependencyNode node)
        {
            var result = new Dictionary<MethodInfo, DependencyDictionary>();
            var methods = node.ResultType.GetMethods().Where(field => container.InjectAttributeTypes.Any(attributeType => field.GetCustomAttribute(attributeType) != null));

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