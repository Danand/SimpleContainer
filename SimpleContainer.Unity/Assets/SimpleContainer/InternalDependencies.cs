using SimpleContainer.Interfaces;

namespace SimpleContainer
{
    public sealed class InternalDependencies : IInternalDependencies
    {
        public IActivator Activator { get; }

        public ITypeLoader TypeLoader { get; }

        public InternalDependencies(IActivator activator, ITypeLoader typeLoader)
        {
            Activator = activator;
            TypeLoader = typeLoader;
        }
    }
}