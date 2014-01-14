namespace Mono.Cecil.CodeDom.Parser
{
	/// <summary>
	/// Indicates how deep we need to parse method bodies
	/// </summary>
	public enum CodeDomParsingMode
	{
		/// <summary>
		/// Try Catch Finally blocks only
		/// </summary>
		TcfBlocksOnly,

		/// <summary>
		/// Try, Catch, Finally blocks and branches
		/// </summary>
		Branches,

		/// <summary>
		/// Full AST, including assigns, types casting and so on.
		/// </summary>
		FullTree
	}
}

