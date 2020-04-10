using System;
using System.Collections.Generic;

namespace SimpleContainer.Extensions
{
    internal static class EnumerableExtensions
    {
        public static IEnumerable<T> Flatten<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            foreach (T item in source)
            {
                yield return item;

                var children = selector.Invoke(item);

                foreach (T child in children.Flatten(selector))
                {
                    yield return child;
                }
            }
        }
    }
}
