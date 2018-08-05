using System;

namespace SimpleContainer
{
    public sealed partial class SimpleContainer
    {
        public TResult Resolve<TResult>()
        {
            return (TResult)Resolve(typeof(TResult));
        }

        public object Resolve(Type contractType)
        {
            return Resolve(contractType, new object[0]);
        }
    }
}