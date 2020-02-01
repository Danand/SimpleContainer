using System;
using System.Linq;
using System.Reflection;

namespace SimpleContainer
{
    internal sealed class ArrayArgumentConverter
    {
        public object[] Convert(Type contractType, object[] args)
        {
            if (!contractType.IsArray)
                return args;

            var elementType = contractType.GetElementType();
            var convertMethod = GetType().GetMethod(nameof(GetConvertedArgs), BindingFlags.NonPublic | BindingFlags.Instance)?.MakeGenericMethod(elementType);

            return (object[])convertMethod?.Invoke(this, new object[] { args });
        }

        private object[] GetConvertedArgs<TContract>(object[] args)
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