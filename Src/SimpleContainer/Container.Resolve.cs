using System;
using System.Linq;

namespace SimpleContainer
{
    public sealed partial class Container
    {
        public TContract Resolve<TContract>()
        {
            return (TContract)Resolve(typeof(TContract));
        }

        public TContract[] ResolveMultiple<TContract>()
        {
            return ResolveMultiple(typeof(TContract)).Cast<TContract>().ToArray();
        }

        public object Resolve(Type contractType)
        {
            return Resolve(contractType, new object[0]);
        }

        public object Resolve(Type contractType, params object[] args)
        {
            var contractIsArray = contractType.IsArray;
            var elementType = contractIsArray ? contractType.GetElementType() : contractType;

            var instances = ResolveMultiple(elementType, args);

            if (contractIsArray)
                return instances;

            return instances[0];
        }
    }
}