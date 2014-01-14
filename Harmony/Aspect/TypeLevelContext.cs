using Mono.Cecil;
using System.Collections.Generic;

namespace Harmony.Aspect
{
	/// <summary>
	/// Weaving context
	/// </summary>
	public class TypeLevelContext
	{
		private TypeDefinition _def_harmonyType;

		public TypeLevelContext(Harmony.Sdk.Project project, TypeDefinition def_target)
		{
			Project = project;
			TargetType = def_target;
			Module = TargetType.Module;
			Cache = new Dictionary<string, object>();
		}

		public Dictionary<string, object> Cache { get; protected set; }

		public ModuleDefinition Module { get; protected set; }

		public TypeDefinition TargetType { get; protected set; }

		public Harmony.Sdk.Project Project { get; protected set; }

		public TypeDefinition HarmonyType
		{
			get
			{
				return _def_harmonyType ?? (_def_harmonyType = CreateHarmonyType(TargetType)); 
			}
		}

		TypeDefinition CreateHarmonyType(TypeDefinition targetType)
		{
			var def_type = new TypeDefinition(targetType.Namespace , "HarmonyType" , TypeAttributes.Class | TypeAttributes.NestedPrivate);

			targetType.NestedTypes.Add(def_type);
			def_type.DeclaringType = targetType;
			_def_harmonyType = def_type;

			return def_type;
		}
	}
}

