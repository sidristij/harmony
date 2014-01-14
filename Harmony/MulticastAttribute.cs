using System;

namespace Harmony.Sdk
{
    /// <summary>
    /// Multicast attribute is base class for aspects and advices in the library.
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class MulticastAttribute : Attribute
    {

        /// <summary>
        /// Gets or sets targets types avaliable for this attribute.
        /// </summary>
        /// <value>The targets.</value>
        public AspectTargets Targets { get; set; }

        /// <summary>
        /// Gets or sets target members name pattern. If pattern starts with "regex:" string, RegEx pattern matching will be used.
        /// </summary>
        public string TargetMembers { get; set; }
    }
}

