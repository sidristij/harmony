using System;
using Mono.Cecil.Cil;
using Mono.Cecil.CodeDom.Collections;
using Mono.Cecil.CodeDom.Rocks;

namespace Mono.Cecil.CodeDom.Parser.Arrays
{
	public sealed class ArraySetItemExpression : CodeDomSingleInstructionExpression
	{
		public const int ArrayPos = 0;
		public const int IndexPos = 1;
		public const int ValuePos = 2;
		public const int MaxNodes = 3;

		public ArraySetItemExpression(Context context, Instruction position, TypeReference itemType, CodeDomExpression exp_array, CodeDomExpression exp_index, CodeDomExpression exp_value)
			: base(context, position)
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

			if(!itemType.HardEquals(exp_value.ReturnType))
			{
				throw new ArgumentException(string.Format("exp_value should have same type as all other parameters types (exp_value: {0}, others: {1})", exp_value.ReturnType, exp_index.ReturnType));
			}

			// base class
			ReturnType = itemType;
			ReadsStack = 2;
			WritesStack = 0;
			Nodes = new FixedList<CodeDomExpression>(MaxNodes);

			ReturnType = TypeRef.Of(typeof(void), exp_value.MethodBody.Method.Module);

			// this
			ItemType = itemType;
			ArrayExpression = exp_array;
			IndexExpression = exp_index;
			ValueExpression = exp_value;
		}

		public TypeReference ItemType { get; private set; }

		public CodeDomExpression ArrayExpression { get { return Nodes[ArrayPos]; } set { Nodes[ArrayPos] = value; value.ParentNode = this; } }

		public CodeDomExpression IndexExpression { get { return Nodes[IndexPos]; } set { Nodes[IndexPos] = value; value.ParentNode = this; } }

		public CodeDomExpression ValueExpression { get { return Nodes[ValuePos]; } set { Nodes[ValuePos] = value; value.ParentNode = this; } }

		public override string ToString()
		{
			return string.Format("{0}[{1}] = {2}{3}", ArrayExpression, IndexExpression, ValueExpression, FinalString);
		}
	}
}

namespace Mono.Cecil.CodeDom.Parser
{
	using Arrays;

	static partial class CodeDom
	{
		public static ArraySetItemExpression ArraySetItem(Context context, Instruction position, TypeReference itemType, CodeDomExpression exp_array, CodeDomExpression exp_index, CodeDomExpression exp_value)
		{
			return new ArraySetItemExpression(context, position, itemType, exp_array, exp_index, exp_value);
		}
	}
}

