using Mono.Cecil.CodeDom.Collections;

namespace Mono.Cecil.CodeDom.Parser.Branching
{
	public sealed class CodeDomCaseExpression : CodeDomExpression
	{
		public const int BodyPos = 0;
		public const int MaxNodes = 1;

		public CodeDomCaseExpression(Context context, int[] values, CodeDomExpression exp_body) : base(context)
		{
			// base class
			ReturnType = Context.MakeRef(typeof(void));
			WritesStack = 0;
			ReadsStack = 0;
			Nodes = new FixedList<CodeDomExpression>(MaxNodes);

			// this
			Body = exp_body;
			Values = values;
		}

		public int[] Values { get; private set; }

		public CodeDomExpression Body { get { return Nodes[BodyPos]; } set { Nodes[BodyPos] = value; value.ParentNode = this; } }

		public override string ToString()
		{
			return Body.ToString();
		}
	}
}