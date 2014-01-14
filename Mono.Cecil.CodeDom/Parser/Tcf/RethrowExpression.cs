using Mono.Cecil.Cil;
using Mono.Cecil.CodeDom.Collections;

namespace Mono.Cecil.CodeDom.Parser.Tcf
{
	public sealed class RethrowExpression : CodeDomSingleInstructionExpression
	{
		public const int MaxNodes = 0;

		public RethrowExpression(Context context, Instruction position)
			: base(context, position)
		{
			// base class
			ReturnType = Context.MakeRef(typeof(void));
			WritesStack = 0;
			ReadsStack = 0;
			Nodes = new FixedList<CodeDomExpression>();
		}

		public override string ToString()
		{
			return string.Format("throw{0}", FinalString);
		}
	}
}

namespace Mono.Cecil.CodeDom.Parser
{
	using Tcf;

	public static partial class CodeDom
	{
		public static RethrowExpression Rethrow(Context context, Instruction position)
		{
			return new RethrowExpression(context, position);
		}
	}
}