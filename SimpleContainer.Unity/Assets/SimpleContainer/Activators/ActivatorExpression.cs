using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

using SimpleContainer.Interfaces;

namespace SimpleContainer.Activators
{
    internal sealed class ActivatorExpression : IActivator
    {
        private delegate object ExpressionActivator(params object[] args);

        private const string ARGS_NAME = "args";

        private readonly IActivator activator;
        private readonly IConstructorCacher constructorCacher;
        private readonly Dictionary<ConstructorInfo, ExpressionActivator> activators = new Dictionary<ConstructorInfo, ExpressionActivator>();

        public ActivatorExpression(IConstructorCacher constructorCacher)
        {
            activator = this;
            this.constructorCacher = constructorCacher;
        }

        object IActivator.CreateInstance(Type type)
        {
            var constructor = constructorCacher.GetConstructor(type);
            return activator.CreateInstance(constructor, new object[0]);
        }

        object IActivator.CreateInstance(Type type, object[] args)
        {
            var constructor = constructorCacher.GetConstructor(type);
            return activator.CreateInstance(constructor, args);
        }

        object IActivator.CreateInstance(ConstructorInfo constructor, object[] args)
        {
            if (!activators.TryGetValue(constructor, out var expressionActivator))
            {
                expressionActivator = GetActivator(constructor);
                activators.Add(constructor, expressionActivator);
            }

            return expressionActivator.Invoke(args);
        }

        private ExpressionActivator GetActivator(ConstructorInfo constructor)
        {
            var parameters = constructor.GetParameters();
            var parameterExpression = Expression.Parameter(typeof(object[]), ARGS_NAME);
            var argsExpression = new Expression[parameters.Length];

            for (var i = 0; i < parameters.Length; i++)
            {
                var index = Expression.Constant(i);
                var parameterType = parameters[i].ParameterType;
                Expression parameterAccessorExpression = Expression.ArrayIndex(parameterExpression, index);
                Expression parameterCastExpression = Expression.Convert(parameterAccessorExpression, parameterType);

                argsExpression[i] = parameterCastExpression;
            }

            var newExpression = Expression.New(constructor, argsExpression);
            var lambda = Expression.Lambda(typeof(ExpressionActivator), newExpression, parameterExpression);
            var result = (ExpressionActivator)lambda.Compile();

            return result;
        }
    }
}