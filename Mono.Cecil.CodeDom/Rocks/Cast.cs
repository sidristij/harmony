using System;
using System.Collections.Generic;
using Mono.Cecil.Cil;

namespace Mono.Cecil.CodeDom.Rocks
{ 
    /// <summary>
    /// Should be used for automatic types casting. For example, in flattening CodeDom.
    /// </summary>
    public static class Cast
    {
        private static IEnumerable<Instruction> Empty = new List<Instruction>(0);

        public static IEnumerable<Instruction> To(FieldReference member, TypeReference type, ModuleDefinition context)
        {   
            return Cast.To(member, from: TypeRef.Of(member), to: TypeRef.Of(type, context), context: context);
        }

        public static IEnumerable<Instruction> To(ParameterReference member, TypeReference type, ModuleDefinition context)
        {    
            return Cast.To(member, from: TypeRef.Of(member), to: TypeRef.Of(type, context), context: context);
        }

        public static IEnumerable<Instruction> To(VariableReference member, TypeReference type, ModuleDefinition context)
        {    
            return Cast.To(member, from: TypeRef.Of(member), to: TypeRef.Of(type, context), context:  context);
        }

        private static IEnumerable<Instruction> To(Object member, TypeReference from, TypeReference to, ModuleDefinition context)
        {
            if(from.Equals(to))
            {
                return Empty;
            } else if(IsBetweenRefTypes(from, to))
            {
                return BetweenRefTypes(member,from, to);
            } else if(IsBoxing(from, to)) 
            {
                return Cast.Boxing(member, from, to);
            } else if(IsUnboxing(from, to))
            {
                return Cast.Unboxing(member,from, to);
            } else if(IsBetweenValueTypes(from, to))
            {
                return Cast.BetweenValueTypes(member, from, to);
            }

            throw new InvalidCastException(string.Format("Unable to cast from {0} to {1}", from, to));
        }

        public static bool IsAvaliable(TypeReference from, TypeReference to)
        {
            return IsBetweenRefTypes(from, to) || IsBoxing(from, to) ||
                   IsUnboxing(from, to) || IsBetweenValueTypes(from, to);
        }

        #region Catsing types

        private static bool IsBoxing(TypeReference from, TypeReference to)
        {
            return from.IsValueType && !to.IsValueType && to.Name == "Object" && to.Namespace == "System";
        }

        private static IEnumerable<Instruction> Boxing(object member, TypeReference from, TypeReference to)
        {
            if(IsBoxing(from, to))
            {
                yield return Instruction.Create(OpCodes.Box, to);
            } else {
                throw new InvalidCastException("invalid casting: parameter should be ValueType");
            }
        }

        private static bool IsUnboxing(TypeReference from, TypeReference to)
        {
            return to.IsValueType && !from.IsValueType && from.Name == "Object" && from.Namespace == "System";
        }

        private static IEnumerable<Instruction> Unboxing(object member, TypeReference from, TypeReference to)
        {
            if(IsUnboxing(from, to))
            {
                yield return Instruction.Create(OpCodes.Unbox_Any, to);
            } else {
                throw new InvalidCastException("invalid casting: parameter should be ValueType");
            }
        }

        private static bool IsBetweenValueTypes(TypeReference from, TypeReference to)
        {
            return true;
        }

        private static IEnumerable<Instruction> BetweenValueTypes(object member, TypeReference from, TypeReference to)
        {
            return null;
        }
                
        private static bool IsBetweenRefTypes(TypeReference from, TypeReference to)
        {
            return true;
        }

        private static IEnumerable<Instruction> BetweenRefTypes(object member, TypeReference from, TypeReference to)
        {
            return null;
        }

        #endregion
    }
}

