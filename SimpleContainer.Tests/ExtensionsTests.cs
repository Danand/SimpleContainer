using System;
using System.Linq;

using NUnit.Framework;

using SimpleContainer.Extensions;

namespace SimpleContainer.Tests
{
    [TestFixture]
    public class ExtensionsTests
    {
        [Test]
        public void Flatten_Circular_Pass()
        {
            var array = new[] { 0, 1, 2, 3 };
            _ = array.Flatten(_ => array).TakeWhile(x => x != 3).ToArray();
            Assert.Pass();
        }
    }
}