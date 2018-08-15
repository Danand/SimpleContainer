using System;
using System.Linq;
using System.Reflection;

namespace SimpleContainer
{
    internal class ArrayArgumentConverter
    {
        private readonly Type contractType; // TODO: must be constructor argument type, not type itself!

        public ArrayArgumentConverter(Type contractType)
        {
            this.contractType = contractType;
        }

        public object[] GetConvertedArgs(object[] args)
        {
            var convertMethod = GetType().GetMethod(nameof(Convert), BindingFlags.NonPublic | BindingFlags.Instance)?.MakeGenericMethod(contractType);
            return (object[])convertMethod?.Invoke(this, new object[] { args });
        }

        private object[] Convert<TContract>(object[] args)
        {
            var argsCount = args.Length;
            var result = new object[argsCount];

            for (var i = 0; i < argsCount; i++)
            {
                if (args[i] is object[] resolvableArray && resolvableArray.All(item => item is TContract))
                    result[i] = resolvableArray.Cast<TContract>().ToArray();
                else
                    result[i] = args[i];
            }

            return result;
        }
    }
}