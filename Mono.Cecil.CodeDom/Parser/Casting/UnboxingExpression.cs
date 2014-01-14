using System;
using Mono.Cecil.Cil;
using Mono.Cecil.CodeDom.Collections;

namespace Mono.Cecil.CodeDom.Parser.Casting
{
	public sealed class UnboxingExpression : CodeDomSingleInstructionExpression
	{
		public const int ValuePos = 0;
		public const int MaxNodes = 1;

		public UnboxingExpression(Context context, Instruction position, CodeDomExpression exp_value)
			: base(context, position)
		{
			if(!(position.Operand as TypeReference).IsValueType)
			{
				throw new ArgumentException("val should be value type");
			}

			// base class
			Nodes = new FixedList<CodeDomExpression>(MaxNodes);
			ReturnType = position.Operand as TypeReference;
			ReadsStack = 1;
			WritesStack = 1;
			Value = exp_value;
		}

		public CodeDomExpression Value { get { return Nodes[ValuePos]; } set { Nodes[ValuePos] = value; value.ParentNode = this; } }

		public override string ToString()
		{
			return string.Format("(({1}){0})", Value, ReturnType);
		}
	}
}

namespace Mono.Cecil.CodeDom.Parser
{
	using Casting;

	public static partial class CodeDom
	{
		public static UnboxingExpression Unboxing(Context context, Instruction position, CodeDomExpression exp_value)
		{
			return new UnboxingExpression(context, position, exp_value);
		}
	}
}
