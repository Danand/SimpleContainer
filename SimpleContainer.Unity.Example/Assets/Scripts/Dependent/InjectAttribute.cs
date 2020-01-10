using System;

namespace SimpleContainer.Unity.Example.Dependent
{
    /// <summary>
    /// Custom attribute used for property and field injection.
    /// Notice it's located in project core assembly, not in DI-container assembly!
    /// </summary>
    public sealed class InjectAttribute : Attribute { }
}