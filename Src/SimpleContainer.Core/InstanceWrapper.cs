using System.Collections.Generic;

namespace SimpleContainer
{
    internal sealed class InstanceWrapper
    {
        public InstanceWrapper(object value)
        {
            Value = value;
        }

        public object Value { get; }

        public bool IsInitialized { get; set; }

        public static bool operator ==(InstanceWrapper first, InstanceWrapper second)
        {
            return EqualityComparer<InstanceWrapper>.Default.Equals(first, second);
        }

        public static bool operator !=(InstanceWrapper first, InstanceWrapper second)
        {
            return !(first == second);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var wrapper = obj as InstanceWrapper;
            return wrapper != null && EqualityComparer<object>.Default.Equals(Value, wrapper.Value);
        }
    }
}