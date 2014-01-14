using Mono.Cecil.Cil;

namespace Mono.Cecil.CodeDom.Parser.Branching
{
	public class CodeDomBranchingExpression : CodeDomExpression
	{
		public CodeDomBranchingExpression(Context context, Instruction target) : base(context)
		{
			BranchingInstruction = target;
			context.SetExpression(target, this);
		}

		public Instruction BranchingInstruction { get; private set; }

		/// <inheritdoc/>
		public override bool IsBranches
		{
			get { return true; }
		}
	}
}
