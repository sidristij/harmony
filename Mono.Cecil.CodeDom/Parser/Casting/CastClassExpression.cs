using Mono.Cecil.Cil;
using Mono.Cecil.CodeDom.Collections;

namespace Mono.Cecil.CodeDom.Parser.Casting
{
	public class CastClassExpression : CodeDomCastingExpression
	{
		public const int InstancePos = 0;
		public const int MaxNodes = 1;

		public CastClassExpression(Context context, Instruction position, CodeDomExpression exp_instance)
			: base(context, position)
		{
			var typeto = position.Operand as TypeReference;
			// base class
			ReadsStack = 1;
			WritesStack = 1;
			Nodes = new FixedList<CodeDomExpression>(MaxNodes);
			ReturnType = typeto;

			// this
			InstanceExpression = exp_instance;
			CastTo = typeto;
		}

		public CodeDomExpression InstanceExpression  { get { return Nodes[InstancePos]; } set { Nodes[InstancePos] = value;  value.ParentNode = this; } }

		public override string ToString()
		{
			return string.Format("({1})({0})", InstanceExpression, ReturnType);
		}
	}
}

namespace Mono.Cecil.CodeDom.Parser
{
	using Casting;

	public static partial class CodeDom
	{
		public static CastClassExpression CastClass(Context context, Instruction position, CodeDomExpression exp_instance)
		{
			return new CastClassExpression(context, position, exp_instance);
		}
	}
}
