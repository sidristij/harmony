using Mono.Cecil.Cil;
using Mono.Cecil.CodeDom.Collections;

namespace Mono.Cecil.CodeDom.Parser.Tcf
{
	public sealed class EndFilterExpression : CodeDomSingleInstructionExpression
	{
		public const int ValuePos = 0;
		public const int MaxNodes = 1;

		public EndFilterExpression(Context context, Instruction position, CodeDomExpression exp_instance)
			: base(context, position)
		{
			// base class
			ReturnType = exp_instance.ReturnType;
			WritesStack = 1;
			ReadsStack = 1;
			Nodes = new FixedList<CodeDomExpression>(MaxNodes);

			// this
			Value = exp_instance;
		}

		public CodeDomExpression Value { get { return Nodes[ValuePos]; } set { Nodes[ValuePos] = value; value.ParentNode = this; } }

		public override string ToString()
		{
			return string.Format("/* endfilter: {0} */ {1}", Value, FinalString);
		}
	}
}

namespace Mono.Cecil.CodeDom.Parser
{
	using Tcf;

	public static partial class CodeDom
	{
		public static EndFilterExpression EndFilter(Context context, Instruction position, CodeDomExpression exp_value)
		{
			return new EndFilterExpression(context, position, exp_value);
		}
	}
}