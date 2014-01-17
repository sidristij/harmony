using Mono.Cecil.Cil;
using Mono.Cecil.CodeDom.Collections;

namespace Mono.Cecil.CodeDom.Parser.SimpleExpressions
{
	public sealed class NotExpression : CodeDomSingleInstructionExpression
	{
		public const int ValuePos = 0;
		public const int MaxNodes = 1;

		public NotExpression(Context context, Instruction position, CodeDomExpression exp_value)
			: base(context, position)
		{
			// base class
			ReturnType = exp_value.ReturnType;
			WritesStack = 1;
			ReadsStack = 1;
			Nodes = new FixedList<CodeDomExpression>(MaxNodes);

			// this
			Value = exp_value;
		}

		public CodeDomExpression Value { get { return Nodes[ValuePos]; } set { Nodes[ValuePos] = value; value.ParentNode = this; } }

		public override string ToString()
		{
			return string.Format("~{0}", Value);
		}
	}
}

namespace Mono.Cecil.CodeDom.Parser
{
	using SimpleExpressions;

	public static partial class CodeDom
	{
		public static NotExpression Not(Context context, Instruction position, CodeDomExpression exp_value)
		{
			return new NotExpression(context, position, exp_value);
		}
	}
}