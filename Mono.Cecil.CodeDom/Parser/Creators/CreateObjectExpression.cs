using System;
using Mono.Cecil.Cil;
using Mono.Cecil.CodeDom.Parser.Branching;
using Mono.Cecil.CodeDom.Rocks;

namespace Mono.Cecil.CodeDom.Parser.Creators
{
	public class CreateObjectExpression : MethodCallExpression
	{
		public CreateObjectExpression(Context context, Instruction position, MethodReference ctorMethod, params CodeDomExpression[] exp_methodParams)
			: base(context, position, ctorMethod, new CodeDomExpression(context), exp_methodParams)
		{
			if (ctorMethod.Name != ".ctor")
			{
				throw new ArgumentException("ctorMethod should be an object constructor");
			}

			IsConstructor = true;
			ReturnType = ctorMethod.DeclaringType;
		}

		public override string ToString()
		{
			return 
				ReturnType.IsArray 
					? string.Format("new {0}[{1}]", MethodReference.DeclaringType.GetElementType(), string.Join(", ", Parameters))
					: string.Format("new {0}({1})", MethodReference.DeclaringType, string.Join(", ", Parameters));
		}
	}
}

namespace Mono.Cecil.CodeDom.Parser
{
	using Creators;

	public static partial class CodeDom
	{
		public static CreateObjectExpression CreateObject(Context context, Instruction position, MethodReference ctorMethod, params CodeDomExpression[] exp_methodParams)
		{
			return new CreateObjectExpression(context, position, ctorMethod, exp_methodParams);
		}
	}
}