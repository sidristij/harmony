using Mono.Cecil.Cil;

namespace Mono.Cecil.CodeDom
{
	public class CodeDomSingleInstructionExpression : CodeDomExpression
	{
		public CodeDomSingleInstructionExpression(Context context, Instruction target) : base(context)
		{
			Instruction = target;
			context.SetExpression(target, this);
		}

		public Instruction Instruction { get; private set; }

		public override bool IsParsed
		{
			get { return true; }
		}

		public override bool IsEmpty
		{
			get { return false; }
		}
	}
}

