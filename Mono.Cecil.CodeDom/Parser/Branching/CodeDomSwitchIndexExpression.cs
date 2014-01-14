using Mono.Cecil.CodeDom.Collections;

namespace Mono.Cecil.CodeDom.Parser.Branching
{
	public sealed class CodeDomSwitchIndexExpression : CodeDomExpression
	{
		public const int IndexPos = 0;
		public const int MaxNodes = 1;

		public CodeDomSwitchIndexExpression(Context context, CodeDomExpression exp_index)
			: base(context)
		{
			// base class
			ReturnType = Context.MakeRef(typeof(void));
			WritesStack = 0;
			ReadsStack = 0;
			Nodes = new FixedList<CodeDomExpression>(MaxNodes);

			// this
			Index = exp_index;
		}

		public CodeDomExpression Index { get { return Nodes[IndexPos]; } set { Nodes[IndexPos] = value; value.ParentNode = this; } }

		public override string ToString()
		{
			return Index.ToString();
		}
	}
}