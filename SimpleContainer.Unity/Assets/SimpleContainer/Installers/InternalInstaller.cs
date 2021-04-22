using SimpleContainer.Activators;
using SimpleContainer.Interfaces;
using SimpleContainer.TypeLoaders;

namespace SimpleContainer.Installers
{
    internal sealed class InternalInstaller : IInstaller
    {
        public void Install(Container container)
        {
            container.Register<IActivator>(Scope.Singleton, new ActivatorReflection());
            container.Register<ITypeLoader>(Scope.Singleton, new TypeLoaderReflection());
            container.Register<IInternalDependencies, InternalDependencies>(Scope.Singleton);
        }
    }
}