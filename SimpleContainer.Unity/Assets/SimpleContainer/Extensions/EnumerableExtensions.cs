using System;
using System.Collections.Generic;

namespace SimpleContainer.Extensions
{
    internal static class EnumerableExtensions
    {
        public static IEnumerable<T> Flatten<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> selector)
        {
            if (source == null)
                yield break;

            var queue = new Queue<T>(source);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                yield return current;

                if (current == null)
                    continue;

                var children = selector.Invoke(current);

                if (children == null)
                    continue;

                foreach (var child in children)
                    queue.Enqueue(child);
            }
        }
    }
}
