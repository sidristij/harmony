using Mono.Cecil.Cil;

namespace Mono.Cecil.CodeDom.Parser.Branching
{
	enum BrCmp {
		// Br:       // Unconditional
		// Brfalse:  // if false, 0, null
		// Brtrue:   // if true, <>0, <>null

		Beq,      // if 2 values equal
		Bge,      // if first >= second
		Bgt,      // if first > second
		Ble,      // if first <= second
		Blt,      // if first < second
		Bne_Un,   // if unsigned1 != unsigned2
		Bge_Un,   // if unsigned >= unsigned2
		Bgt_Un,   // if unsigned > unsigned2
		Ble_Un,   // if unsigned <= unsigned2
		Blt_Un,   // if unsigned < unsigned2
	}
	public class CodeDomComparingBranchExpression : CodeDomBranchingExpression
	{
		public CodeDomComparingBranchExpression(Context context, Instruction target, CodeDomExpression exp_condition, CodeDomExpression exp_ontrue, CodeDomExpression exp_onfalse) : base(context, target)
		{
			Condition = exp_condition;
			TrueExpression = exp_ontrue;
			FalseExpression = exp_onfalse;
		}

		public CodeDomExpression Condition { get; private set; }

		public CodeDomExpression TrueExpression { get; private set; }

		public CodeDomExpression FalseExpression { get; private set; }
	}
}

