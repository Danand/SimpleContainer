using System;

using SimpleContainer.Factories;

namespace SimpleContainer.Tests.DummyTypes
{
    public class CatFactory : Factory<ICat>
    {
        public override Type GetResultType(Type resultType, params object[] args)
        {
            return typeof(GingerCat);
        }
    }
}