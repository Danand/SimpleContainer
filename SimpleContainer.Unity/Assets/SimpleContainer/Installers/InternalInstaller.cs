using SimpleContainer.Activators;
using SimpleContainer.Interfaces;
using SimpleContainer.TypeLoaders;

namespace SimpleContainer.Installers
{
    internal sealed class InternalInstaller : IInstaller
    {
        public void Install(Container container)
        {
            container.Register<IInternalDependencies>(Scope.Singleton, new InternalDependencies(new ActivatorReflection(), new TypeLoaderReflection()));
        }
    }
}