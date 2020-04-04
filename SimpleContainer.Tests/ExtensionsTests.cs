using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using SimpleContainer.Extensions;
using SimpleContainer.Tests.DummyTypes;

namespace SimpleContainer.Tests
{
    [TestFixture]
    public class ExtensionsTests
    {
        [Test]
        public void Flatten_Circular_Pass()
        {
            var node = new Node();
            var nodes = new List<Node> { node };

            node.Nodes = nodes;

            var result = nodes.Flatten(x => x.Nodes, (x, y) => x == y).ToArray();

            Assert.AreEqual(1, result.Length);
        }
    }
}