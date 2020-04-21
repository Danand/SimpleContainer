using UnityEngine;
using UnityEngine.SceneManagement;

namespace SimpleContainer.Unity.Utils
{
    public static class ComponentUtils
    {
        public static T FindInAllLoadedScenes<T>()
            where T : MonoBehaviour
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);

                foreach (var rootGameObject in scene.GetRootGameObjects())
                {
                    var result = rootGameObject.GetComponentInChildren<T>();

                    if (result != null)
                        return result;
                }
            }

            return null;
        }
    }
}