using System;
using Mono.Cecil.Cil;
using Mono.Cecil.CodeDom.Collections;
using Mono.Cecil.CodeDom.Rocks;

namespace Mono.Cecil.CodeDom.Parser.Members
{
	public sealed class ParameterSetExpression : CodeDomSingleInstructionExpression
	{
		public const int ValuePos = 0;
		public const int MaxNodes = 1;

		public ParameterSetExpression(Context context, Instruction position, ParameterReference ref_parameter, CodeDomExpression exp_value)
			: base(context, position)
		{   
			if(ref_parameter.Index >= short.MaxValue) 
			{
				throw new ArgumentOutOfRangeException(string.Format("Method must not contain more than {0} parameters. Please, optimize your code.", short.MaxValue));
			}

			if(!ref_parameter.ParameterType.HardEquals(exp_value.ReturnType))
			{
				throw new InvalidOperationException(string.Format("parameter type '{0}' is not equals to value type '{1}'", ref_parameter.ParameterType.FullName, exp_value.ReturnType.FullName));
			}

			// base class
			ReadsStack = 1;
			WritesStack = 0;
			Nodes = new FixedList<CodeDomExpression>(MaxNodes);

			ReturnType = Context.MakeRef(typeof(void));

			// this
			ParameterReference = ref_parameter;
			ValueExpression = exp_value;
		}

		public ParameterReference ParameterReference { get; private set; }

		public CodeDomExpression ValueExpression { get { return Nodes[ValuePos]; } set { Nodes[ValuePos] = value; value.ParentNode = this; } }

		public override string ToString()
		{
			return string.Format("{0} = {1}{2}", ParameterReference, ValueExpression, FinalString);
		}
	}
}

namespace Mono.Cecil.CodeDom.Parser
{
	using Members;

	public static partial class CodeDom
	{
		public static ParameterSetExpression ParameterSet(Context context, Instruction position, ParameterReference ref_parameter, CodeDomExpression exp_value)
		{
			return new ParameterSetExpression(context, position, ref_parameter, exp_value);
		}
	}
}
