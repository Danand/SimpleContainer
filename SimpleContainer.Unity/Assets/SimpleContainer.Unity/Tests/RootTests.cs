using NUnit.Framework;

using SimpleContainer.Unity.Roots;

using UnityEngine;

namespace SimpleContainer.Unity.Tests
{
    public class RootTests
    {
        [Test]
        public void OverrideFrom_Pass()
        {
            var projectRoot = new GameObject().AddComponent<UnityProjectRootManual>();
            var sceneRoot = new GameObject().AddComponent<UnitySceneRootManual>();

            // TODO: Add real checks.

            Assert.NotNull(projectRoot);
            Assert.NotNull(sceneRoot);
        }
    }
}
