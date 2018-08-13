using System;

namespace SimpleContainer.Factories
{
    public abstract class Factory<TResult> : IFactory
    {
        Container IFactory.Container { get; set; }

        public abstract Type GetResultType(Type resultType, params object[] args);

        public TResult Create(params object[] args)
        {
            var factory = (IFactory)this;

            var contractType = typeof(TResult);
            var resultType = GetResultType(typeof(TResult), args);

            if (!factory.Container.CheckRegistered(contractType))
                factory.Container.Register(contractType, resultType);

            return (TResult)factory.Container.Resolve(contractType, args);
        }
    }
}