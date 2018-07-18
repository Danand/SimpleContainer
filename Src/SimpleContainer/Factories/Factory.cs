using System;

namespace SimpleContainer.Factories
{
    public abstract class Factory<TResult> : IFactory
    {
        SimpleContainer IFactory.Container { get; set; }

        public abstract Type GetResultType(Type resultType, params object[] args);

        public TResult Create(params object[] args)
        {
            var factory = (IFactory)this;
            var resultType = GetResultType(typeof(TResult), args);

            if (!factory.Container.CheckRegistered(resultType))
                factory.Container.Register(resultType);

            return (TResult)factory.Container.Resolve(resultType, args);
        }
    }
}