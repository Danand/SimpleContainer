using System;
using System.Linq;
using System.Reflection;

namespace SimpleContainer
{
    internal sealed class CollectedValue
    {
        private readonly Type returnType;
        private readonly object[] values;

        public CollectedValue(Type returnType, object[] values)
        {
            this.returnType = returnType;
            this.values = values;
        }

        public object Value
        {
            get
            {
                if (returnType == null)
                    throw new NullReferenceException(nameof(returnType));

                var elementType = returnType.IsArray ? returnType.GetElementType() : returnType;

                if (returnType.IsArray)
                    return CastArray(values, elementType);

                return values[0];
            }
        }

        private object CastArray(object[] sourceArray, Type elementType)
        {
            var resultArray = Array.CreateInstance(elementType, sourceArray.Length);
            Array.Copy(sourceArray, resultArray, sourceArray.Length);

            return resultArray;
        }
    }
}