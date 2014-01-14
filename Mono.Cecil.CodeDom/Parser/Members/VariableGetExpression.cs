using System;
using Mono.Cecil.Cil;
using Mono.Cecil.CodeDom.Collections;

namespace Mono.Cecil.CodeDom.Parser.Members
{
	public sealed class VariableGetExpression : CodeDomSingleInstructionExpression
	{
		const int MaximumVariablesAlloved = 65534;

		public VariableGetExpression(Context context, Instruction position, VariableReference ref_variable)
			: base(context, position)
		{   
			if(ref_variable.Index >= MaximumVariablesAlloved) 
			{
				throw new ArgumentOutOfRangeException(string.Format("Method must not contain more than {0} variables. Please, optimize your code.", MaximumVariablesAlloved));
			}

			// base class
			ReadsStack = 0;
			WritesStack = 1;
			Nodes = new FixedList<CodeDomExpression>();

			// this
			VariableReference = ref_variable;
			ReturnType = VariableReference.VariableType;
		}

		public VariableReference VariableReference { get; private set; }

		public override string ToString()
		{
			return string.Format("{0}", VariableReference);
		}
	}
}

namespace Mono.Cecil.CodeDom.Parser
{
	using Members;

	public static partial class CodeDom
	{
		public static VariableGetExpression VariableGet(Context context, Instruction position, VariableReference ref_variable)
		{
			return new VariableGetExpression(context, position, ref_variable);
		}
	}
}
