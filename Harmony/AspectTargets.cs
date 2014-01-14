using System;

namespace Harmony.Sdk
{
    [Flags]
    public enum AspectTargets
    {
        Default   = Any,
        
        // Other
        Assembly  = 0x00000001,
        Module    = 0x00000002,
        
        // Types
        Class     = 0x00000100,
        Struct    = 0x00000200,
        Enum      = 0x00000400,
        Delegate  = 0x00000800,
        Interface = 0x00001000,
        
        // Members  
        Field     = 0x00010000,
        Method    = 0x00020000,
        InstConstructor  = 0x00040000,
        StaticConstructor= 0x00080000,
        Property  = 0x00100000,
        Event     = 0x00200000,
        
        // Other
        Variable = 0x01000000,
        Parameter = 0x02000000,
        ReturnValue = 0x04000000,
        
        // Grouping
        AsmOrModule     = Assembly | Module,
        AnyMethod       = Method | Property | Event,
        AnyConstructor  = InstConstructor | StaticConstructor,
        
        // Masks
        AnyMemberContent= 0x7f000000,
        AnyMember       = 0x00ff0000,
        AnyType         = 0x0000ff00,
        AnyHierarchy    = 0x000000ff,
        Any             = AnyType | AnyMember | AnyMemberContent | AnyHierarchy
    }
}

