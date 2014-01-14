using System;
using Mono.Cecil.Cil;
using Mono.Cecil.CodeDom.Collections;

namespace Mono.Cecil.CodeDom.Parser.Members
{
	public sealed class ParameterGetExpression : CodeDomSingleInstructionExpression
	{
		public const int MaxNodes = 0;

		public ParameterGetExpression(Context context, Instruction position, ParameterReference ref_parameter)
			: base(context, position)
		{   
			if(ref_parameter.Index >= short.MaxValue) 
			{
				throw new ArgumentOutOfRangeException(string.Format("Method must not contain more than {0} parameters. Please, optimize your code.", short.MaxValue));
			}

			// base class
			ReadsStack = 0;
			WritesStack = 1;
			ReturnType = ref_parameter.ParameterType;
			Nodes = new FixedList<CodeDomExpression>();

			// this
			ParameterReference = ref_parameter;
		}

		public ParameterReference ParameterReference  { get; private set; }

		public override string ToString()
		{
			return string.Format("{0}", ParameterReference);
		}
	}
}

namespace Mono.Cecil.CodeDom.Parser
{
	using Members;

	public static partial class CodeDom
	{
		public static ParameterGetExpression ParameterGet(Context context, Instruction position, ParameterReference ref_parameter)
		{
			return new ParameterGetExpression(context, position, ref_parameter);
		}
	}
}


