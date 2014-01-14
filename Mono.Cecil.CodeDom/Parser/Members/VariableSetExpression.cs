using System;
using Mono.Cecil.Cil;
using Mono.Cecil.CodeDom.Collections;
using Mono.Cecil.CodeDom.Rocks;

namespace Mono.Cecil.CodeDom.Parser.Members
{
	public sealed class VariableSetExpression : CodeDomSingleInstructionExpression
	{
		public const int ValuePos = 0;
		public const int MaxNodes = 1;

		const int MaximumVariablesAlloved = 65534;

		public VariableSetExpression(Context context, Instruction position, VariableReference ref_variable, CodeDomExpression exp_value)
			: base(context, position)
		{   
			if(ref_variable.Index >= MaximumVariablesAlloved) 
			{
				throw new ArgumentOutOfRangeException(string.Format("Method must not contain more than {0} variables. Please, optimize your code.", MaximumVariablesAlloved));
			}

			if(!ref_variable.VariableType.HardEquals(exp_value.ReturnType))
			{
				//throw new InvalidOperationException(string.Format("variable type '{0}' is not equals to value type '{1}'", ref_variable.VariableType.FullName, exp_value.ReturnType.FullName));
			}

			// base class
			ReadsStack = 1;
			WritesStack = 0;
			ReturnType = Context.MakeRef(typeof(void));
			Nodes = new FixedList<CodeDomExpression>(MaxNodes);

			// this
			VariableReference = ref_variable;
			ValueExpression = exp_value;
		}

		public VariableReference VariableReference { get; private set; }

		public CodeDomExpression ValueExpression { get { return Nodes[ValuePos]; } set { Nodes[ValuePos] = value; value.ParentNode = this; } }

		public override string ToString()
		{
			return string.Format("{0} = {1}", VariableReference, ValueExpression);
		}
	}
}

namespace Mono.Cecil.CodeDom.Parser
{
	using Members;

	public static partial class CodeDom
	{
		public static VariableSetExpression VariableSet(Context context, Instruction position, VariableReference ref_variable, CodeDomExpression exp_value)
		{
			return new VariableSetExpression(context, position, ref_variable, exp_value);
		}
	}
}
