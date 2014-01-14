using Mono.Cecil.Cil;
using Mono.Cecil.CodeDom.Collections;

namespace Mono.Cecil.CodeDom.Parser.Tcf
{
	public sealed class CkfiniteExpression : CodeDomSingleInstructionExpression
	{
		public const int ValuePos = 0;
		public const int MaxNodes = 1;

		public CkfiniteExpression(Context context, Instruction position, CodeDomExpression exp_value)
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
			return string.Format("if(float.IsNan({0}) || float.IsInfinite({0}) throw new ArithmeticException();{1}", Value, FinalString);
		}
	}
}

namespace Mono.Cecil.CodeDom.Parser
{
	using Tcf;

	public static partial class CodeDom
	{
		public static CkfiniteExpression Ckfinite(Context context, Instruction position, CodeDomExpression exp_value)
		{
			return new CkfiniteExpression(context, position, exp_value);
		}
	}
}