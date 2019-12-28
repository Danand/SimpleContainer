using System;

using SimpleContainer.Interfaces;

namespace SimpleContainer
{
    public sealed class AnyArgs : IEventArgs
    {
        private readonly IEventArgs innerArgs;

        public AnyArgs(IEventArgs innerArgs)
        {
            this.innerArgs = innerArgs;
        }

        public TEventArgs GetInnerArgs<TEventArgs>()
            where TEventArgs : IEventArgs
        {
            return (TEventArgs)innerArgs;
        }

        public Type GetInnerType()
        {
            return innerArgs.GetType();
        }
    }
}