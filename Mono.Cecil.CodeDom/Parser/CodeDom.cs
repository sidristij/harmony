using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Mono.Cecil.CodeDom.Parser
{
	public static partial class CodeDom
	{
		#region Native Pointers (8)

		public static CodeDomSingleInstructionExpression PtrCastToPointer(Instruction position, CodeDomExpression ref_instance)
		{
			return null;
		}

		public static CodeDomSingleInstructionExpression PtrCastToMethodHandler(Instruction position, MethodReference ref_method, CodeDomExpression exp_methodPointer)
		{
			return null;
		}

		public static CodeDomSingleInstructionExpression PtrCastToValue(Instruction position, CodeDomExpression exp_pointer, TypeReference ref_type)
		{
			return null;
		}

		public static CodeDomSingleInstructionExpression PtrCopyFromToValue(Instruction position, TypeReference ref_type, CodeDomExpression exp_source, CodeDomExpression destination)
		{
			return null;
		}

		public static CodeDomSingleInstructionExpression PtrSetValue(Instruction position, CodeDomExpression exp_address, CodeDomExpression exp_instance)
		{
			return null;
		}

		public static CodeDomSingleInstructionExpression PtrMemAlloc(Instruction position, CodeDomExpression exp_allocsize)
		{
			return null;
		}

		public static CodeDomSingleInstructionExpression PtrMemInit(Instruction position, CodeDomExpression exp_address, CodeDomExpression exp_initvalue, CodeDomExpression exp_bytesnum)
		{
			return null;
		}

		public static CodeDomSingleInstructionExpression PtrMemCopy(Instruction position, CodeDomExpression exp_source, CodeDomExpression exp_destination, CodeDomExpression exp_bytesToCopy)
		{
			return null;
		}

		#endregion

		#region Misc (5)

		public static CodeDomSingleInstructionExpression SkippedInstruction(Instruction position)
		{
			return null;
		}

		public static CodeDomExpression UncoveredInstruction(Context context, Instruction position)
		{
			return new CodeDomExpression(context);
		}

		#endregion
	}
}