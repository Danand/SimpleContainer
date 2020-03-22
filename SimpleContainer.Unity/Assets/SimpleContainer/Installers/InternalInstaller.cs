using SimpleContainer.Activators;
using SimpleContainer.Interfaces;
using SimpleContainer.TypeLoaders;

namespace SimpleContainer.Installers
{
    internal sealed class InternalInstaller : IInstaller
    {
        public void Install(Container container)
        {
            container.Register<IActivator, ActivatorReflection>(Scope.Singleton);
            container.Register<ITypeLoader, TypeLoaderReflection>(Scope.Singleton);
            container.Register<IInternalDependencies, InternalDependencies>(Scope.Singleton);
        }
    }
}