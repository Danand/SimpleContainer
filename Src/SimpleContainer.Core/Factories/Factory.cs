namespace SimpleContainer.Factories
{
    public abstract class Factory<TContract> : IFactory
    {
        Container IFactory.Container { get; set; }

        public TContract Create<TResult>(params object[] args)
        {
            var factory = (IFactory)this;
            var contractType = typeof(TContract);
            var resultType = typeof(TResult);

            if (!factory.Container.CheckRegistered(contractType))
                factory.Container.Register(contractType, resultType);

            return (TContract)factory.Container.Resolve(contractType, args);
        }
    }
}