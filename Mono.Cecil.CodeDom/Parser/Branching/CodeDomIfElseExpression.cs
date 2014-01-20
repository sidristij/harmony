using Mono.Cecil.Cil;
using Mono.Cecil.CodeDom.Collections;

namespace Mono.Cecil.CodeDom.Parser.Branching
{
	public sealed class CodeDomIfElseExpression : CodeDomBranchingExpression
	{
		public const int ConditionPos = 0;
		public const int TrueExpressionPos = 1;
		public const int FalseExpressionPos = 2;
		public const int MaxNodes = 3;

		public CodeDomIfElseExpression(Context context, Instruction target, CodeDomConditionExpression exp_condition, CodeDomExpression exp_ontrue, CodeDomExpression exp_onfalse) : base(context, target)
		{
			Nodes = new FixedList<CodeDomExpression>(MaxNodes);
			Context.SetExpression(target, this);
			Condition = exp_condition;
			TrueExpression = exp_ontrue;
			FalseExpression = exp_onfalse;
		}

		public CodeDomConditionExpression Condition { get { return (CodeDomConditionExpression) Nodes[ConditionPos]; } private set { Nodes[ConditionPos] = value; value.ParentNode = this; } }

		public CodeDomExpression TrueExpression { get { return Nodes[TrueExpressionPos]; } private set { Nodes[TrueExpressionPos] = value; value.ParentNode = this; } }

		public CodeDomExpression FalseExpression { get { return Nodes[FalseExpressionPos]; } private set { Nodes[FalseExpressionPos] = value; value.ParentNode = this; } }

		public override string ToString()
		{
			var sb = new System.Text.StringBuilder();
			sb.Append("if(");
			sb.Append(Condition.ToString());
			sb.Append(") {");
			sb.AppendLine();
			sb.Append(TrueExpression.ToString());
			sb.AppendLine("}");
			if (!FalseExpression.IsEmpty)
			{
				sb.AppendLine("else {");
				sb.Append(FalseExpression.ToString());
				sb.AppendLine("}");
			}
			return sb.ToString();
		}
	}
}

