using System;

namespace SimpleContainer
{
    public static class ReflectionUtils
    {
        public static object CastNonGeneric(object source, Type type)
        {
            return typeof(ReflectionUtils).GetMethod(nameof(Cast))
                                          .MakeGenericMethod(type)
                                          .Invoke(null, new [] { source });
        }

        public static T Cast<T>(object source)
        {
            return (T)source;
        }
    }
}