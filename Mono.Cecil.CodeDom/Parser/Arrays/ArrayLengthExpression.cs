using System;
using Mono.Cecil.Cil;
using Mono.Cecil.CodeDom.Collections;

namespace Mono.Cecil.CodeDom.Parser.Arrays
{
	public sealed class ArrayLengthExpression : CodeDomSingleInstructionExpression
	{
		public const int ArrayPos = 0;
		public const int MaxNodes = 1;

		public ArrayLengthExpression(Context context, Instruction position, TypeReference itemType, CodeDomExpression exp_array) 
			: base(context, position)
		{
			if(!(exp_array.ReturnType is ArrayType)) 
			{
				throw new ArgumentException(string.Format("exp_array is not array ({0})", exp_array.ReturnType));
			}

			// setup parents
			exp_array.ParentNode = this;

			// base class
			ReadsStack = 1;
			WritesStack = 1;
			ReturnType = context.MakeRef<int>();
			Nodes = new FixedList<CodeDomExpression>(MaxNodes);

			// this
			ItemType = itemType;
			ArrayExpression = exp_array;
		}

		public TypeReference ItemType { get; private set; }

		public CodeDomExpression ArrayExpression { get { return Nodes[ArrayPos]; } set { Nodes[ArrayPos] = value; value.ParentNode = this; } }

		public override string ToString()
		{
			return string.Format("{0}.Length", ArrayExpression);
		}
	}
}

namespace Mono.Cecil.CodeDom.Parser
{
	using Arrays;

	public static partial class CodeDom
	{
		public static ArrayLengthExpression ArrayLength(Context context, Instruction position, TypeReference itemType, CodeDomExpression exp_array)
		{
			return new ArrayLengthExpression(context, position, itemType, exp_array);
		}
	}
}

