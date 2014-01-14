using Mono.Cecil;
using System.Collections.Generic;

namespace Harmony.Sdk
{
	/// <summary>
	/// Project is a set of assemblies which should be weaved in same scope
	/// </summary>
	public class Project
	{
		private readonly Dictionary<TypeDefinition, TypeDefinition> _utilsClasses = new Dictionary<TypeDefinition, TypeDefinition>();

		private readonly List<FileDefinition> _aspectsAssemblies = new List<FileDefinition>();

		private readonly List<FileDefinition> _inputAssemblies = new List<FileDefinition>();

		private readonly DefaultAssemblyResolver _assemblyResolver;

		public List<FileDefinition> AspectsAssemblies { get { return _aspectsAssemblies; } }

		public List<FileDefinition> InputAssemblies { get { return _inputAssemblies; } }
	}
}

