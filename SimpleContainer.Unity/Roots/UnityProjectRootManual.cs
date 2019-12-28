using System.Threading.Tasks;

namespace SimpleContainer.Unity.Roots
{
    public sealed class UnityProjectRootManual : UnityProjectRootBase
    {
        public async Task InstallAsync()
        {
            await InstallAsyncInternally();
        }
    }
}