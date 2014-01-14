using System;
using Mono.Cecil.Cil;
using Mono.Cecil.CodeDom.Collections;

namespace Mono.Cecil.CodeDom.Parser.Casting
{
	public sealed class BoxingExpression : CodeDomSingleInstructionExpression
	{
		public const int ValuePos = 0;
		public const int MaxNodes = 1;

		public BoxingExpression(Context context, Instruction position, CodeDomExpression exp_value)
			: base(context, position)
		{
			if(!exp_value.ReturnType.IsValueType)
			{
				throw new ArgumentException("val should be value type");
			}

			// base class
			Nodes = new FixedList<CodeDomExpression>(MaxNodes);
			ReturnType = Context.MakeRef<object>();
			ReadsStack = 1;
			WritesStack = 1;
			Value = exp_value;
		}

		public CodeDomExpression Value { get { return Nodes[ValuePos]; } set { Nodes[ValuePos] = value; value.ParentNode = this; } }

		public override string ToString()
		{
			return string.Format("((object)({0}))", Value);
		}
	}
}

namespace Mono.Cecil.CodeDom.Parser
{
	using Casting;

	public static partial class CodeDom
	{
		public static BoxingExpression Boxing(Context context, Instruction position, CodeDomExpression exp_value)
		{
			return new BoxingExpression(context, position, exp_value);
		}
	}
}
