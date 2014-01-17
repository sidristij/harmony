using System;
using Mono.Cecil.Cil;
using Mono.Cecil.CodeDom.Collections;
using Mono.Cecil.CodeDom.Rocks;

namespace Mono.Cecil.CodeDom.Parser.Creators
{
	public sealed class InitStructExpression : CodeDomSingleInstructionExpression
	{
		public const int InstancePos = 0;
		public const int MaxNodes = 1;

		public InitStructExpression(Context context, Instruction position, CodeDomExpression exp_instance, TypeReference ref_type)
			: base(context, position)
		{
			if(exp_instance.WritesStack != 1)
			{
				throw new InvalidOperationException(string.Format("Wrong stack items count in <exp_length> argument: {0}", exp_instance.WritesStack));
			}
			if(!exp_instance.ReturnType.IsValueType)
			{
				throw new ArgumentException(string.Format("<exp_instance> should be ValueType ({0})", exp_instance.ReturnType));
			}

			if(!Cast.IsAvaliable(exp_instance.ReturnType, ref_type))
			{
				throw new InvalidCastException(string.Format("<exp_instance> ({0}) type cannot be initialized as <ref_type> ({1})", exp_instance, ref_type));
			}

			// base class
			ReadsStack = 1;
			ReturnType = Context.MakeRef(typeof(void));
			Nodes = new FixedList<CodeDomExpression>(MaxNodes);

			// this
			InitAsType = ref_type;
			InstanceExpression = exp_instance;
		}

		public TypeReference InitAsType { get; private set; }

		public CodeDomExpression InstanceExpression { get { return Nodes[InstancePos]; } set { Nodes[InstancePos] = value; value.ParentNode = this; } }

		public override string ToString()
		{
			return string.Format("var {1} = new {0}()", InitAsType, InstanceExpression);
		}
	}
}

namespace Mono.Cecil.CodeDom.Parser
{
	using Creators;

	public static partial class CodeDom
	{
		public static InitStructExpression InitStruct(Context context, Instruction position, CodeDomExpression exp_instance, TypeReference ref_type)
		{
			return new InitStructExpression(context, position, exp_instance, ref_type);
		}

		public static InitStructExpression InitStruct(Context context, Instruction position, CodeDomExpression exp_instance)
		{
			return new InitStructExpression(context, position, exp_instance, exp_instance.ReturnType);
		}
	}
}