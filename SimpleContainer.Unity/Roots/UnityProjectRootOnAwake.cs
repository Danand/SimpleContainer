namespace SimpleContainer.Unity.Roots
{
    public class UnityProjectRootOnAwake : UnityProjectRootBase
    {
        async void Awake()
        {
            await InstallAsyncInternally();
        }
    }
}