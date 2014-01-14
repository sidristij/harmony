using Mono.Cecil.Cil;

namespace Mono.Cecil.CodeDom
{
	public class CodeDomCastingExpression : CodeDomSingleInstructionExpression 
	{
		public CodeDomCastingExpression(Context context, Instruction position) : base(context, position)
		{
		}

		public TypeReference CastTo { get; protected set; }
	}
}
