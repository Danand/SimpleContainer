using SimpleContainer.Interfaces;
using SimpleContainer.Unity.Events;

namespace SimpleContainer.Unity.Installers
{
    public class MonoCallbacksInstaller : IInstaller
    {
        void IInstaller.Install(Container container)
        {
            container.RegisterEvent<UpdateArgs>();
            container.RegisterEvent<FixedUpdateArgs>();
        }
    }
}