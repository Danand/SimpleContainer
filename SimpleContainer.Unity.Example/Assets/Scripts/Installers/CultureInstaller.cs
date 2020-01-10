using SimpleContainer.Unity.Example.Dependent.CultureInfoProviders;
using SimpleContainer.Unity.Example.Dependent.Interfaces;
using SimpleContainer.Unity.Example.Dependent.TimeZoneProviders;
using SimpleContainer.Unity.Installers;

namespace SimpleContainer.Unity.Example.Installers
{
    public sealed class CultureInstaller : MonoInstaller
    {
        public override void Install(Container container)
        {
            container.Register<ITimeZoneProvider, TimeZoneProviderDefault>(Scope.Singleton);
            container.Register<ITimeZoneProvider, TimeZoneProviderMSK>(Scope.Singleton);
            container.Register<ITimeZoneProvider, TimeZoneProviderJST>(Scope.Singleton);
            container.Register<ITimeZoneProvider, TimeZoneProviderBLDRN>(Scope.Singleton);
            container.Register<ITimeZoneProvider, TimeZoneProviderBTTF>(Scope.Singleton);

            container.Register<ICultureInfoFormatter, CultureInfoFormatterJP>(Scope.Singleton);
        }
    }
}
