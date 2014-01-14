using Mono.Cecil.Cil;
using Mono.Cecil.CodeDom.Collections;

namespace Mono.Cecil.CodeDom.Parser.Casting
{
	public class IsInstanceExpression : CodeDomSingleInstructionExpression
	{
		public const int InstancePos = 0;
		public const int MaxNodes = 1;

		public IsInstanceExpression(Context context, Instruction position, CodeDomExpression exp_instance)
			: base(context, position)
		{
			// base class
			ReadsStack = 1;
			WritesStack = 1;
			Nodes = new FixedList<CodeDomExpression>(MaxNodes);
			ReturnType = Context.MakeRef<bool>();

			// this
			InstanceExpression = exp_instance;
		}

		public CodeDomExpression InstanceExpression  { get { return Nodes[InstancePos]; } set { Nodes[InstancePos] = value; value.ParentNode = this; } }

		public override string ToString()
		{
			return string.Format("{0} is {1}", InstanceExpression, ReturnType);
		}
	}
}

namespace Mono.Cecil.CodeDom.Parser
{
	using Casting;

	public static partial class CodeDom
	{
		public static IsInstanceExpression IsInstanceOf(Context context, Instruction position, CodeDomExpression exp_instance)
		{
			return new IsInstanceExpression(context, position, exp_instance);
		}
	}
}
