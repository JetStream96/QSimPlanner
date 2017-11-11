using System;

namespace CommonLibrary.Attributes
{
    /// <summary>
    /// Indicates that the method may throw exception but the exception type is not important.
    /// </summary>
    public class Throws : Attribute { }

    /// <summary>
    /// Indicates that the method cannot throw exception.
    /// </summary>
    public class NoThrow : Attribute { }
}
