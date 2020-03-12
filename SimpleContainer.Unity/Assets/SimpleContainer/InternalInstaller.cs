using SimpleContainer.Activators;
using SimpleContainer.Interfaces;
using SimpleContainer.TypeLoaders;

namespace SimpleContainer
{
    internal sealed class InternalInstaller : IInstaller
    {
        public void Install(Container container)
        {
            container.Register<IActivator, ActivatorReflection>(Scope.Singleton, new ActivatorReflection());
            container.Register<ITypeLoader, TypeLoaderReflection>(Scope.Singleton);
            container.Register<IInternalDependencies, InternalDependencies>(Scope.Singleton);
        }
    }
}