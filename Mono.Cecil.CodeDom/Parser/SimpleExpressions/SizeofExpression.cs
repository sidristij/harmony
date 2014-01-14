﻿using Mono.Cecil.Cil;
using Mono.Cecil.CodeDom.Collections;

namespace Mono.Cecil.CodeDom.Parser.SimpleExpressions
{
	public sealed class SizeofExpression : CodeDomSingleInstructionExpression
	{
		public SizeofExpression(Context context, Instruction position)
			: base(context, position)
		{
			// base class
			ReturnType = Context.MakeRef<int>();
			WritesStack = 1;
			ReadsStack = 0;
			Nodes = new FixedList<CodeDomExpression>();
		}

		public override string ToString()
		{
			return string.Format("sizeof({0})", ReturnType);
		}
	}
}

namespace Mono.Cecil.CodeDom.Parser
{
	using SimpleExpressions;

	public static partial class CodeDom
	{
		public static SizeofExpression Sizeof(Context context, Instruction position)
		{
			return new SizeofExpression(context, position);
		}
	}
}