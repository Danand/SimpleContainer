using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

using SimpleContainer.Interfaces;

namespace SimpleContainer.Activators
{
    public sealed class ActivatorExpression : IActivator
    {
        private delegate object ExpressionActivator(params object[] args);

        private readonly Dictionary<ConstructorInfo, ExpressionActivator> expressionActivators = new Dictionary<ConstructorInfo, ExpressionActivator>();

        object IActivator.CreateInstance(ConstructorInfo constructor, object[] args)
        {
            if (!expressionActivators.TryGetValue(constructor, out var expressionActivator))
            {
                expressionActivator = GetActivator(constructor);
                expressionActivators.Add(constructor, expressionActivator);
            }

            return expressionActivator.Invoke(args);
        }

        private ExpressionActivator GetActivator(ConstructorInfo constructor)
        {
            var parameters = constructor.GetParameters();
            var parameterExpression = Expression.Parameter(typeof(object[]), "args");
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