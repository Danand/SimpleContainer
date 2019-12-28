namespace SimpleContainer.Unity.Roots
{
    public class UnityProjectRootOnAwake : UnityProjectRootBase
    {
        protected override async void Awake()
        {
            base.Awake();
            await InstallAsyncInternally();
        }
    }
}