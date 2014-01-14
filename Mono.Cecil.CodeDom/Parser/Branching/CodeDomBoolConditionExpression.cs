using Mono.Cecil.CodeDom.Collections;

namespace Mono.Cecil.CodeDom.Parser.Branching
{
	public sealed class CodeDomBoolConditionExpression : CodeDomExpression
	{
		public const int ConditionPos = 0;
		public const int MaxNodes = 1;

		public CodeDomBoolConditionExpression(Context context, CodeDomExpression exp_condition) : base(context)
		{
			ReadsStack = 1;
			WritesStack = 0;

			Nodes = new FixedList<CodeDomExpression>(MaxNodes);

			ConditionExpression = exp_condition;
		}

		public CodeDomExpression ConditionExpression { get { return Nodes[ConditionPos]; } set { Nodes[ConditionPos] = value; value.ParentNode = this; } }

		public override string ToString()
		{
			return ConditionExpression.ToString();
		}
	}
}

