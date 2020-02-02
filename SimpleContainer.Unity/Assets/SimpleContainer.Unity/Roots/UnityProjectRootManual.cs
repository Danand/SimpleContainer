namespace SimpleContainer.Unity.Roots
{
    public sealed class UnityProjectRootManual : UnityProjectRootBase
    {
        public async void InstallAsync()
        {
            await InstallAsyncInternally();
        }
    }
}