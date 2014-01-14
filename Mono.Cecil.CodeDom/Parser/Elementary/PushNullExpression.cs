using Mono.Cecil.Cil;
using Mono.Cecil.CodeDom.Collections;

namespace Mono.Cecil.CodeDom.Parser.Elementary
{
	public sealed class PushNullExpression : CodeDomSingleInstructionExpression
	{
		public PushNullExpression(Context context, Instruction position)
			: base(context, position)
		{
			// base class
			ReturnType = Context.MakeRef<object>();
			WritesStack = 1;
			Nodes = new FixedList<CodeDomExpression>();
		}

		public override string ToString()
		{
			return string.Format("null");
		}
	}
}

namespace Mono.Cecil.CodeDom.Parser
{
	using Elementary;

	public static partial class CodeDom
	{
		public static PushNullExpression PushNull(Context context, Instruction position)
		{
			return new PushNullExpression(context, position);
		}
	}
}


