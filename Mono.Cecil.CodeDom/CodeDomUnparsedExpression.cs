using Mono.Cecil.Cil;

namespace Mono.Cecil.CodeDom
{
	public class CodeDomUnparsedExpression : CodeDomExpression
	{
		public CodeDomUnparsedExpression(Context context, Instruction from, Instruction to ) : base(context)
		{
			Instructions = new InstructionsRange { First = from, Last = to };
			ResetInstructionsInMap();
		}

		public override void ResetInstructionsInMap()
		{
			foreach (var instruction in Instructions)
			{
				Context.SetExpression(instruction, this);
			}
		}

		public InstructionsRange Instructions { get; private set; }

		public override bool IsEmpty
		{
			get { return false; }
		}

		public override bool IsParsed
		{
			get { return false; }
		}

		public override string ToString()
		{
			return string.Format("/*Unparsed: 0x{0:X02}..0x{1:X02} ({2})*/", Instructions.First.Offset, Instructions.Last.Offset, GetHashCode());
		}
	}
}

