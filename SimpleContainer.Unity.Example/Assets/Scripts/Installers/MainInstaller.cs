using System.Threading.Tasks;

using SimpleContainer.Unity.Installers;

using SimpleContainer.Unity.Example.Dependent;
using SimpleContainer.Unity.Example.Dependent.CultureInfoProviders;
using SimpleContainer.Unity.Example.Dependent.Interfaces;
using SimpleContainer.Unity.Example.Dependent.Loggers;
using SimpleContainer.Unity.Example.Dependent.TimeZoneProviders;

namespace SimpleContainer.Unity.Example.Installers
{
    public sealed class MainInstaller : MonoInstaller
    {
        public UIManager uiManager;

        public override void Install(Container container)
        {
            container.RegisterAttribute<InjectAttribute>();

            container.Register(Scope.Singleton, uiManager);

            container.Register<ITimeZoneProvider, TimeZoneProviderDefault>(Scope.Singleton);
            container.Register<ITimeZoneProvider, TimeZoneProviderMSK>(Scope.Singleton);
            container.Register<ITimeZoneProvider, TimeZoneProviderJST>(Scope.Singleton);
            container.Register<ITimeZoneProvider, TimeZoneProviderBLDRN>(Scope.Singleton);
            container.Register<ITimeZoneProvider, TimeZoneProviderBTTF>(Scope.Singleton);

            container.Register<ILogger, LoggerDefault>(Scope.Singleton);
            container.Register<ICultureInfoFormatter, CultureInfoFormatterJP>(Scope.Singleton);
        }

        public override Task AfterResolveAsync(Container container)
        {
            uiManager.Initialize();
            return Task.CompletedTask;
        }
    }
}
