using System.Collections;

using NUnit.Framework;

using SimpleContainer.Unity.Roots;
using SimpleContainer.Unity.Tests.DummyClasses;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace SimpleContainer.Unity.Tests
{
    [TestFixture]
    public class RootTests
    {
        [UnityTest]
        public IEnumerator OverrideFrom_Pass()
        {
            SceneManager.LoadScene(0, LoadSceneMode.Single);

            yield return null;

            var projectRoot = Object.FindObjectOfType<UnityProjectRootManual>();

            yield return projectRoot.InstallAsync().AsEnumerator();

            SceneManager.LoadScene(1, LoadSceneMode.Additive);

            yield return null;

            var sceneRoot = Object.FindObjectOfType<UnitySceneRootManual>();

            yield return sceneRoot.InstallAsync().AsEnumerator();

            var supportManager = sceneRoot.Container.Resolve<ISupportManager>() as SupportManager;

            Assert.IsNotNull(supportManager?.LocalizationRepository);
        }
    }
}
