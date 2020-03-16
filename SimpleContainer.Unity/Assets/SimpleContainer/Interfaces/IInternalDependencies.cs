namespace SimpleContainer.Interfaces
{
    internal interface IInternalDependencies
    {
        IActivator Activator { get; }

        ITypeLoader TypeLoader { get; }
    }
}