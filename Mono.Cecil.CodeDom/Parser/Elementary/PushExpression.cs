using System;
using Mono.Cecil.Cil;
using Mono.Cecil.CodeDom.Collections;

namespace Mono.Cecil.CodeDom.Parser.Elementary
{
	public sealed class PushExpression<T> : CodeDomSingleInstructionExpression
	{
		public PushExpression(Context context, Instruction position, T val)
			: base(context, position)
		{
			var ref_type = Context.MakeRef<T>();

			if(!ref_type.IsPrimitive && ref_type.MetadataType != MetadataType.String)
			{
				throw new ArgumentException("val should have primitive type");
			}

			// base class
			ReturnType = ref_type;
			WritesStack = 1;
			Value = val;
			Nodes = new FixedList<CodeDomExpression>();

			// this
			ItemType = ref_type;
		}

		public T Value { get; private set; }

		public TypeReference ItemType { get; private set; }

		public override string ToString()
		{
			if (Value is string)
			{
				return string.Format("\"{0}\"", Value);
			}
			else if (Value is Single)
			{
				return string.Format("{0:f}f", Value);
			}
			else if (Value is Double)
			{
				return string.Format("{0:f}", Value);
			}

			return string.Format("{0}", Value);
		}
	}
}

namespace Mono.Cecil.CodeDom.Parser
{
	using Elementary;

	public static partial class CodeDom
	{
		public static PushExpression<T> Push<T>(Context context, Instruction position, T val)
		{
			return new PushExpression<T>(context, position, val);
		}
	}
}