using System;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleContainer.Unity.Example.Extensions
{
    public static class AsyncExtensions
    {
        public static async Task ReactAsync<T1, T2>(this T1 source, Func<T1, T2> selector, Action<T2> callback, CancellationTokenSource cts = null)
        {
            T2 lastValue = selector.Invoke(source);

            callback.Invoke(lastValue);

            while (true)
            {
                if (cts?.IsCancellationRequested ?? false)
                    break;

                T2 currentValue = selector.Invoke(source);

                if (!lastValue.Equals(currentValue))
                {
                    lastValue = currentValue;
                    callback.Invoke(lastValue);
                }

                await Task.Yield();
            }
        }

    }
}