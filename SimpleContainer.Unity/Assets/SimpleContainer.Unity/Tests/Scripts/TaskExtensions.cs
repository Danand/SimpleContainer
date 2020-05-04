using System;
using System.Collections;
using System.Threading.Tasks;

namespace SimpleContainer.Unity.Tests
{
    public static class TaskExtensions
    {
        public static IEnumerator AsEnumerator(this Task source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            while (!(source.IsCompleted || source.IsFaulted))
            {
                yield return null;
            }

            if (source.IsFaulted)
            {
                throw source.Exception ?? new Exception("Task faulted");
            }
        }
    }
}