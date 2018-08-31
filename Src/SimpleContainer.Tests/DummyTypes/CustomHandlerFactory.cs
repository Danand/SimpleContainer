using System;

using SimpleContainer.Factories;

namespace SimpleContainer.Tests.DummyTypes
{
    public sealed class CustomHandlerFactory : Factory<ICustomHandler>
    {
        public override Type GetResultType(Type resultType, params object[] args)
        {
            if (args != null && args.Length == 1)
                return (Type)args[0];

            return typeof(CustomHandler);
        }
    }
}