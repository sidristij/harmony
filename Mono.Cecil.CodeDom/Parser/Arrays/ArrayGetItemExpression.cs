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

		public ArrayGetItemExpression(Context context, Instruction position, TypeReference itemType, 
									  CodeDomExpression exp_array, CodeDomExpression exp_index) : base(context, position)
		{
			if(!(exp_array.ReturnType is ArrayType))
			{
				throw new ArgumentException(string.Format("exp_array is not array ({0})", exp_array.ReturnType));
			}

			var arrayItemType = (exp_array.ReturnType as ArrayType).ElementType;
			if(!arrayItemType.HardEquals(itemType))
			{
				throw new ArgumentException(string.Format("exp_array items type should be equal to itemType type (items: {0}, itemType: {1})", arrayItemType, itemType));
			}

			if(exp_index.ReturnType.MetadataType.GetTypeCode() != TypeCode.Int32)
			{
				throw new ArgumentException(string.Format("exp_index should be Int16 ({0})", exp_index.ReturnType));
			}

			// base class
			ReturnType = itemType;
			ReadsStack = 2;
			WritesStack = 1;
			Nodes = new FixedList<CodeDomExpression>(MaxNodes);

			// this
			ItemType = itemType;
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
		public static ArrayGetItemExpression ArrayGetItem(Context context, Instruction position, TypeReference itemType, CodeDomExpression exp_array, CodeDomExpression exp_index)
		{
			return new ArrayGetItemExpression(context, position, itemType, exp_array, exp_index);
		}
	}
}

