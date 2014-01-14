
namespace Harmony.Sdk.Config
{
    using System;public enum ConfigOverlappingAction
    {
        Ignore,
        Allow
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class ConfigOverlapsAttribute : Attribute
    {


        public ConfigOverlapsAttribute(ConfigOverlappingAction action)
        {
            Action = action;
        }

        public ConfigOverlappingAction Action
        {
            get;
            protected set;
        }
    }
}

