using System;
using Mono.Cecil.Cil;
using Mono.Cecil.CodeDom.Collections;
using Mono.Cecil.CodeDom.Rocks;

namespace Mono.Cecil.CodeDom.Parser.Arrays
{   
	public sealed class ArrayGetItemExpression : CodeDomSingleInstructionExpression
	{
		public const int ArrayPos = 0;
		public const int IndexPos = 1;
		public const int MaxNodes = 2;

		public ArrayGetItemExpression(Context context, Instruction position, CodeDomExpression exp_array, CodeDomExpression exp_index) : base(context, position)
		{
			if(!(exp_array.ReturnType is ArrayType))
			{
				throw new ArgumentException(string.Format("exp_array is not array ({0})", exp_array.ReturnType));
			}

			var arrayItemType = (exp_array.ReturnType as ArrayType).ElementType;

			if(exp_index.ReturnType.MetadataType != MetadataType.Int32)
			{
				throw new ArgumentException(string.Format("exp_index should be Int32 ({0})", exp_index.ReturnType));
			}

			// base class
			ReturnType = arrayItemType;
			ReadsStack = 2;
			WritesStack = 1;
			Nodes = new FixedList<CodeDomExpression>(MaxNodes);

			// this
			ItemType = arrayItemType;
			ArrayExpression = exp_array;
			IndexExpression = exp_index;
		}

		public TypeReference ItemType { get; private set; }

		public CodeDomExpression ArrayExpression { get { return Nodes[ArrayPos]; } set { Nodes[ArrayPos] = value; value.ParentNode = this; } }

		public CodeDomExpression IndexExpression { get { return Nodes[IndexPos]; } set { Nodes[IndexPos] = value; value.ParentNode = this; } }

		public override string ToString()
		{
			return string.Format("{0}[{1}]", ArrayExpression, IndexExpression);
		}
	}
}

namespace Mono.Cecil.CodeDom.Parser
{
	using Arrays;

	public static partial class CodeDom
	{
		public static ArrayGetItemExpression ArrayGetItem(Context context, Instruction position, CodeDomExpression exp_array, CodeDomExpression exp_index)
		{
			return new ArrayGetItemExpression(context, position, exp_array, exp_index);
		}
	}
}

