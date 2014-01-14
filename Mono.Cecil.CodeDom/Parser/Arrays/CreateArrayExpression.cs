using System;
using Mono.Cecil.Cil;
using Mono.Cecil.CodeDom.Collections;
using Mono.Cecil.Rocks;

namespace Mono.Cecil.CodeDom.Parser.Arrays
{
	public sealed class CreateArrayExpression : CodeDomSingleInstructionExpression
	{
		public const int LengthPos = 0;
		public const int MaxNodes = 1;

		public CreateArrayExpression(Context context, Instruction position, TypeReference itemType, CodeDomExpression exp_length) 
			: base(context, position)
		{
			if(exp_length.WritesStack != 1)
			{
				throw new InvalidOperationException(string.Format("Wrong stack items count in exp_length argument: {0}", exp_length.WritesStack));
			}

			// check is length type is int16
			if(exp_length.ReturnType.MetadataType.GetTypeCode() != TypeCode.Int16)
			{
				throw new ArgumentException(string.Format("exp_length should be System.Int16 ({0})", exp_length.ReturnType));
			}

			// base class
			ReturnType = itemType.MakeArrayType();
			ReadsStack = 1;
			WritesStack = 1;
			Nodes = new FixedList<CodeDomExpression>(MaxNodes);

			// this
			ItemType = itemType;
			LengthExpression = exp_length;
		}

		public TypeReference ItemType { get; private set; }

		public CodeDomExpression LengthExpression { get { return Nodes[LengthPos]; } set { Nodes[LengthPos] = value; value.ParentNode = this; } }

		public override string ToString()
		{
			return string.Format("new {0}[{1}]", ItemType, LengthExpression);
		}
	}
}

namespace Mono.Cecil.CodeDom.Parser
{
	using Arrays;

	public static partial class CodeDom
	{
		public static CreateArrayExpression CreateArray(Context context, Instruction position, TypeReference itemType, CodeDomExpression exp_length)
		{
			return new CreateArrayExpression(context, position, itemType, exp_length);
		}
	}
}

