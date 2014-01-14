using Mono.Cecil.Cil;
using Mono.Cecil.CodeDom.Collections;

namespace Mono.Cecil.CodeDom.Parser.Branching
{
	public enum LoopType
	{
		While,
		DoWhile
	}

	public sealed class CodeDomLoopExpression : CodeDomBranchingExpression
	{
		public const int ConditionPos = 0;
		public const int BodyExpressionPos = 1;
		public const int MaxNodes = 2;

		public CodeDomLoopExpression(Context context, Instruction incomingJump, Instruction loopJump, LoopType looptype, CodeDomExpression exp_condition, CodeDomExpression exp_body) : base(context, loopJump)
		{
			Nodes = new FixedList<CodeDomExpression>(MaxNodes);
			if(incomingJump != null)
				Context.SetExpression(incomingJump, this);
			Condition = exp_condition;
			Body = exp_body;
			LoopType = looptype;
		}

		public LoopType LoopType { get; private set; }

		public CodeDomExpression Condition { get { return Nodes[ConditionPos]; } private set { Nodes[ConditionPos] = value; value.ParentNode = this; } }

		public CodeDomExpression Body { get { return Nodes[BodyExpressionPos]; } private set { Nodes[BodyExpressionPos] = value; value.ParentNode = this; } }

		public override string ToString()
		{
			var sb = new System.Text.StringBuilder();

			if (LoopType == LoopType.DoWhile)
			{
				sb.AppendLine("do {");
				sb.AppendLine(Body.ToString());
				sb.AppendLine(string.Format("}} while ({0});", Condition));
			}
			else
			{
				sb.AppendLine(string.Format("while ({0}) {{", Condition));
				sb.AppendLine(Body.ToString());
				sb.AppendLine("}");
			}
			return sb.ToString();
		}
	}
}

