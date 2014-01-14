using Mono.Cecil.Cil;
using Mono.Cecil.CodeDom.Collections;

namespace Mono.Cecil.CodeDom.Parser.Tcf
{
	public sealed class ThrowExpression : CodeDomSingleInstructionExpression
	{
		public const int InstancePos = 0;
		public const int MaxNodes = 1;

		public ThrowExpression(Context context, Instruction position, CodeDomExpression exp_instance)
			: base(context, position)
		{
			// base class
			ReturnType = exp_instance.ReturnType;
			WritesStack = 1;
			ReadsStack = 1;
			Nodes = new FixedList<CodeDomExpression>(MaxNodes);

			// this
			Instance = exp_instance;
		}

		public CodeDomExpression Instance { get { return Nodes[InstancePos]; } set { Nodes[InstancePos] = value; value.ParentNode = this; } }

		public override string ToString()
		{
			return string.Format("throw {0}{1}", Instance, FinalString);
		}
	}
}

namespace Mono.Cecil.CodeDom.Parser
{
	using Tcf;

	public static partial class CodeDom
	{
		public static ThrowExpression Throw(Context context, Instruction position, CodeDomExpression exp_value)
		{
			return new ThrowExpression(context, position, exp_value);
		}
	}
}