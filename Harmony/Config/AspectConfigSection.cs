using System;
using System.Xml;

namespace Harmony.Sdk.Config
{
	/// <summary>
	/// Aspect config section.
	/// </summary>
	public class AspectConfigSection : ConfigSectionBase
	{
		public AspectConfigSection() : base("Aspect")
		{
		}

		public string AspectName { get; protected set; }

		public AspectTargets Targets { get; protected set; }

		public string TargetMembers { get; protected set; }
	}
}

