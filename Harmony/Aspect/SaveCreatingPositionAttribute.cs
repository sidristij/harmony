using System;

namespace Harmony.Aspect
{
    /// <summary>
    /// Create array expression. Implements Newarr MSIL instruction.
    /// LengthExpression should have natural int type (Int32).
    /// ItemType can be any reference or value type.
    /// </summary>
    public class SaveCreatingPositionAttribute : Attribute
    {
    }
}

