using System;
using Mono.Cecil.Cil;
using Mono.Cecil.CodeDom.Collections;

namespace Mono.Cecil.CodeDom.Parser.Pointers
{
	public sealed class PtrCastToMethodHandlerExpression : CodeDomSingleInstructionExpression
	{
		public PtrCastToMethodHandlerExpression(Context context, Instruction position, MethodReference ref_method)
			: base(context, position)
		{
			// base class
			ReadsStack = 1;
			WritesStack = 1;
			ReturnType = Context.MakeRef<IntPtr>();
			Nodes = new FixedList<CodeDomExpression>();

			// this class
			MethodReference = ref_method;
		}

		public MethodReference MethodReference { get; private set; }

		public override string ToString()
		{
			return string.Format("/*methodof*/{0}", MethodReference.Name);
		}
	}
}

namespace Mono.Cecil.CodeDom.Parser
{
	using Pointers;

	public static partial class CodeDom
	{
		public static PtrCastToMethodHandlerExpression PtrCastToMethodHandler(Context context, Instruction position, MethodReference ref_method)
		{
			return new PtrCastToMethodHandlerExpression(context, position, ref_method);
		}
	}
}