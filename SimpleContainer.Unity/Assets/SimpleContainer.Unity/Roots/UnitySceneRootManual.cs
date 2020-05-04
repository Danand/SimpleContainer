using System.Threading.Tasks;

namespace SimpleContainer.Unity.Roots
{
    public sealed class UnitySceneRootManual : UnitySceneRootBase
    {
        public async Task InstallAsync()
        {
            await InstallAsyncInternally();
        }
    }
}