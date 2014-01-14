using System;
using Harmony.Sdk;
using Mono.Cecil;

namespace Harmony.Aspect
{
	/// <summary>
	/// Base class for all aspects
	/// </summary>
	[AttributeUsage(AttributeTargets.All)]
	public class AspectBase : MulticastAttribute
	{
		public AspectBase()
		{
			SkipWeaving = false;
		}

		public virtual void CheckConditions(MethodDefinition def_method)
		{
		}

		public virtual void Weave(MethodDefinition def_method)
		{
		}

		public bool Retarget(AspectTargets targets, bool removeThis = false)
		{
			return true;
		}

		/// <summary>
		/// Indicates that weaving phase should be skipped. Should be set when CheckCondition have no 
		/// errors or warnings found, but weaving should not be planned
		/// </summary>
		/// <value><c>true</c> if skip weaving; otherwise, <c>false</c>.</value>
		public bool SkipWeaving { get; set; }

		/// <summary>
		/// Gets the weaving context.
		/// </summary>
		/// <value>The context.</value>
		public TypeLevelContext Context { get; private set; }
	}
}

