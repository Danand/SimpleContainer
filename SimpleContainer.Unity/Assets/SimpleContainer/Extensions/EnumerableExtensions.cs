using System;
using System.Collections.Generic;

namespace SimpleContainer.Extensions
{
    internal static class EnumerableExtensions
    {
        private const int CYCLES_MAX = 1000;

        public static IEnumerable<T> Flatten<T>(this IEnumerable<T>     source,
                                                Func<T, IEnumerable<T>> selector,
                                                Func<T, T, bool>        itemComparer,
                                                T                       previousItem = default,
                                                int                     cycleCounter = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            cycleCounter++;

            if (cycleCounter > CYCLES_MAX)
                throw new Exception($"cycleCounter > {CYCLES_MAX}");

            foreach (T item in source)
            {
                if (previousItem != null && itemComparer.Invoke(previousItem, item))
                    continue;

                yield return item;

                var children = selector.Invoke(item);

                foreach (T child in children.Flatten(selector, itemComparer, item, cycleCounter))
                {
                    yield return child;
                }
            }
        }
    }
}
