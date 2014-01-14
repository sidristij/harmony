using System.Collections.Generic;

namespace Mono.Cecil.CodeDom.Rocks
{
    public static class Hierarсhy
    {
        /// <summary>
        /// Enumerates all parents for given nested type
        /// </summary>
        public static IEnumerable<TypeReference> ParentTypes(this TypeReference self)
        {
            self = self.DeclaringType;
            while(self != null)
            {
                yield return self;
                self = self.DeclaringType;
            }
        }

        public static bool InheritedFrom(this TypeReference self, TypeReference another)
        {
            foreach (var type in self.ParentTypes())
            {
                if(type.SoftEquals(another))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks that types names and assemblies names are equal
        /// </summary>
        public static bool SoftEquals(this TypeReference self, TypeReference another)
        {
            var leftRoot = self.GetElementType();
            var rghtRoot = another.GetElementType();

            var lasm = leftRoot.Module.Assembly;
            var rasm = rghtRoot.Module.Assembly;

            return (lasm.Name == rasm.Name && leftRoot.FullName == another.FullName);
        }
        
        /// <summary>
        /// Checks that types names and assemblies names are equal
        /// </summary>
        public static bool HardEquals(this TypeReference self, TypeReference another)
        {
            var leftRoot = self.GetElementType();
            var rghtRoot = another.GetElementType();
            
            var lasm = leftRoot.Module.Assembly;
            var rasm = rghtRoot.Module.Assembly;

            // if type is loaded, assembly is loaded too, so assembly references should be equal
            return (lasm == rasm && leftRoot.FullName == another.FullName);
        }
    }
}

