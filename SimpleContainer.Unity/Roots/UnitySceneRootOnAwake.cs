namespace SimpleContainer.Unity.Roots
{
    public sealed class UnitySceneRootOnAwake : UnitySceneRootBase
    {
        protected override async void Awake()
        {
            base.Awake();
            await InstallAsyncInternally();
        }
    }
}