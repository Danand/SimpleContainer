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
            var node1 = new Node();
            var node2 = new Node();

            var nodes = new List<Node> { node1, node2 };

            node1.Nodes = new List<Node>();
            node2.Nodes = nodes;

            var result = nodes.Flatten(x => x.Nodes, (x, y) => x == y).ToArray();

            Assert.AreEqual(3, result.Length);
        }
    }
}