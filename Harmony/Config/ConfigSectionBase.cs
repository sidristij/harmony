using System;

namespace Harmony.Sdk.Config
{
    public abstract class ConfigSectionBase
    {
        /// <summary>
        /// Abstract class constructor
        /// </summary>
        protected ConfigSectionBase(string sectionName)
        {
            SectionName = sectionName;
        }

        public void InitFrom(IConfigSectionDataProvider dataProvider)
        {

        }

        public string SectionName { get; private set; }
    }
}

