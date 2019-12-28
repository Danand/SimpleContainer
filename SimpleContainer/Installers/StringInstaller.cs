using System.Reflection;

using SimpleContainer.Interfaces;

namespace SimpleContainer.Installers
{
    public sealed class StringInstaller : IInstaller
    {
        private readonly Assembly assembly;
        private readonly string name;

        public StringInstaller(Assembly assembly, string name)
        {
            this.assembly = assembly;
            this.name = name;
        }

        void IInstaller.Install(Container container)
        {
            container.Install(assembly, name);
        }
    }
}