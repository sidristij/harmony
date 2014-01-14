using Mono.Cecil;
using System.Collections.Generic;

namespace Harmony.Aspect
{
    /// <summary>
    /// Weaving context
    /// </summary>
    public class InstanceLevelContext
    {
        public TypeLevelContext TypeLevel { get; protected set; }
    }
}
