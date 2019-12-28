using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

using SimpleContainer.Interfaces;

namespace SimpleContainer.Activators
{
    internal class ActivatorExpression : IActivator
    {
        private delegate object ExpressionActivator(params object[] args);

        private const string ARGS_NAME = "args";

        private readonly Dictionary<ConstructorInfo, ExpressionActivator> activators = new Dictionary<ConstructorInfo, ExpressionActivator>();

        object IActivator.CreateInstance(ConstructorInfo constructor, object[] args)
        {
            if (!activators.TryGetValue(constructor, out var activator))
            {
                activator = GetActivator(constructor);
                activators.Add(constructor, activator);
            }

            return activator.Invoke(args);
        }

        private static ExpressionActivator GetActivator(ConstructorInfo constructor)
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