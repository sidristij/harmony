using Mono.Cecil.Cil;
using Mono.Cecil.CodeDom.Collections;

namespace Mono.Cecil.CodeDom.Parser.Branching
{
	public sealed class ReturnExpression : CodeDomSingleInstructionExpression
	{
		public const int ReturnPos = 0;
		public const int MaxNodes = 1;

		public ReturnExpression(Context context, Instruction position, CodeDomExpression exp_returnValue = null) 
			: base(context, position)
		{
			// base class
			ReadsStack = exp_returnValue != null ? 1 : 0;
			WritesStack = 0;
			Nodes = new FixedList<CodeDomExpression>(MaxNodes);

			// this class
			ReturnValue = exp_returnValue ?? new CodeDomExpression(context);
		}

		public CodeDomExpression ReturnValue { get { return Nodes[ReturnPos]; } private set { Nodes[ReturnPos] = value; value.ParentNode = this; } }

		public override string ToString()
		{
			return string.Format("return{0}{1}", ReadsStack > 0 ? " " + ReturnValue : "", FinalString);
		}
	}
}

namespace Mono.Cecil.CodeDom.Parser
{
	using Branching;

	public static partial class CodeDom
	{
		public static ReturnExpression Return(Context context, Instruction position, CodeDomExpression exp_returnValue = null)
		{
			return new ReturnExpression(context, position, exp_returnValue);
		}
	}
}