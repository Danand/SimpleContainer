using SimpleContainer.Activators;
using SimpleContainer.Interfaces;
using SimpleContainer.TypeLoaders;

namespace SimpleContainer
{
    internal sealed class InternalInstaller : IInstaller
    {
        public void Install(Container container)
        {
            container.Register<IConstructorCacher, ConstructorCacher>(Scope.Singleton);
            container.Register<IActivator, ActivatorReflection>(Scope.Singleton);
            container.Register<ITypeLoader, TypeLoaderReflection>(Scope.Singleton);
        }
    }
}