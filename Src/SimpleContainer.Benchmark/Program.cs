using System;
using System.Diagnostics;

using MyContainer = SimpleContainer.SimpleContainer;

using MicroResolver;

namespace SimpleContainer.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            RunSimpleContainerBenchmark();

            Console.WriteLine();

            RunMicroResolverBenchmark();

            Console.ReadLine();
        }

        private static void RunSimpleContainerBenchmark()
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            var simpleContainer = new MyContainer();

            stopwatch.Stop();

            Console.WriteLine($"{nameof(SimpleContainer)}: creating: {stopwatch.ElapsedMilliseconds} ms");

            stopwatch.Restart();

            simpleContainer.Register<ICar, CarTruck>(Scope.Transient);

            stopwatch.Stop();

            Console.WriteLine($"{nameof(SimpleContainer)}: register transient: {stopwatch.ElapsedMilliseconds} ms");

            stopwatch.Restart();

            simpleContainer.Resolve<ICar>();

            stopwatch.Stop();

            Console.WriteLine($"{nameof(SimpleContainer)}: resolve transient: {stopwatch.ElapsedMilliseconds} ms");
        }

        private static void RunMicroResolverBenchmark()
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            var objectResolver = ObjectResolver.Create();

            stopwatch.Stop();

            Console.WriteLine($"{nameof(ObjectResolver)}: creating: {stopwatch.ElapsedMilliseconds} ms");

            stopwatch.Restart();

            objectResolver.Register<ICar, CarTruck>(Lifestyle.Transient);
            objectResolver.Compile();

            stopwatch.Stop();

            Console.WriteLine($"{nameof(ObjectResolver)}: register transient: {stopwatch.ElapsedMilliseconds} ms");

            stopwatch.Restart();

            objectResolver.Resolve<ICar>();

            stopwatch.Stop();

            Console.WriteLine($"{nameof(ObjectResolver)}: resolve transient: {stopwatch.ElapsedMilliseconds} ms");
        }
    }

    internal interface ICar { }

    internal class CarTruck : ICar { }
}
