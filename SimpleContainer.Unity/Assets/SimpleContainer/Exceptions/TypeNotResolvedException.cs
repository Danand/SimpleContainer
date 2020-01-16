using System;

namespace SimpleContainer.Exceptions
{
    public class TypeNotResolvedException : Exception
    {
        public TypeNotResolvedException(Type type) : base(
            $"Contract type '{type.Name}' is not resolved!{Environment.NewLine}" +
            "Please check unused dependencies or wrong types in expected injections "+
            "(e.g. expected type is less abstract than registered type).") { }
    }
}