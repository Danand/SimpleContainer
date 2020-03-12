using System;
using System.Reflection;

namespace SimpleContainer.Utils
{
    internal static class ReflectionUtils
    {
        public static object CastNonGeneric(object source, Type type)
        {
            return typeof(ReflectionUtils).GetMethod(nameof(Cast), BindingFlags.Static | BindingFlags.NonPublic)?
                                          .MakeGenericMethod(type)
                                          .Invoke(null, new object [] { source });
        }

        public static object CastNonGenericArray(Type type, object[] array)
        {
            return typeof(ReflectionUtils).GetMethod(nameof(CastArray), BindingFlags.Static | BindingFlags.NonPublic)?
                                          .MakeGenericMethod(type)
                                          .Invoke(null, new object [] { array });
        }

        private static T Cast<T>(object source)
        {
            return (T)source;
        }

        private static T[] CastArray<T>(object source)
        {
            var array = (object[])source;
            var result = new T[array.Length];

            for (int i = 0; i < array.Length; i++)
            {
                result[i] = (T)array[i];
            }

            return result;
        }
    }
}