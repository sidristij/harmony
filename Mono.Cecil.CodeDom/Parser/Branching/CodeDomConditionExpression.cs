using Mono.Cecil.CodeDom.Collections;
using Mono.Cecil.CodeDom;
using System;
using System.Linq.Expressions;

namespace Mono.Cecil.CodeDom.Parser.Branching
{
	public sealed class CodeDomConditionExpression : CodeDomExpression
	{
		public const int ConditionPos = 0;
		public const int EvaluationPos = 1;
		public const int MaxNodes = 2;

		public CodeDomConditionExpression(Context context, CodeDomExpression exp_condition) 
			: this(context, exp_condition, new CodeDomExpression(context))
		{
		}

		public CodeDomConditionExpression(Context context, CodeDomExpression exp_condition, CodeDomExpression exp_evaluation) : base(context)
		{
			ReadsStack = 1;
			WritesStack = 0;

			Nodes = new FixedList<CodeDomExpression>(MaxNodes);

			ConditionExpression = exp_condition;
			EvaluationExpression = exp_evaluation;
		}
		
		public CodeDomExpression ConditionExpression { get { return Nodes[ConditionPos]; } set { Nodes[ConditionPos] = value; value.ParentNode = this; } }
		
		public CodeDomExpression EvaluationExpression { get { return Nodes[EvaluationPos]; } set { Nodes[EvaluationPos] = value; value.ParentNode = this; } }

		public override string ToString()
		{
			if (EvaluationExpression.IsEmpty)
			{
				return ConditionExpression.ToString();
			}
			else
			{
				return string.Format("[[{1}]]\r\n{0}", ConditionExpression, EvaluationExpression);
			}
		}
	}
}

