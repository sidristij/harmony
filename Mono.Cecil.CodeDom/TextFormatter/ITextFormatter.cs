namespace Mono.Cecil.CodeDom.TextFormatter
{
	/// <summary>
	/// Interface for all classes whether implements different formatters for different .Net languages.
	/// </summary>
	public interface ITextFormatter
	{
		string Language { get; }
		string FormatMethod(MethodDefinition def_method);
		string FormatCodeDomExpression(CodeDomExpression exp_root);
		string FormatType(TypeDefinition def_type);
	}
}