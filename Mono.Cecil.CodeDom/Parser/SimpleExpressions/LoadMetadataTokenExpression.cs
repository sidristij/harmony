using System;
using System.IO;
using Mono.Cecil.Cil;
using Mono.Cecil.CodeDom.Collections;

namespace Mono.Cecil.CodeDom.Parser.SimpleExpressions
{
	public sealed class LoadMetadataTokenExpression : CodeDomSingleInstructionExpression
	{
		private IMetadataTokenProvider _tokenProvider;

		public LoadMetadataTokenExpression(Context context, Instruction position)
			: base(context, position)
		{
			// base class
			if (position.Operand is TypeReference)
				ReturnType = Context.MakeRef<RuntimeTypeHandle>();
			else if (position.Operand is FieldReference)
				ReturnType = Context.MakeRef<RuntimeFieldHandle>();
			else
				ReturnType = Context.MakeRef<RuntimeMethodHandle>();

			_tokenProvider = position.Operand as IMetadataTokenProvider;

			WritesStack = 1;
			ReadsStack = 0;
			Nodes = new FixedList<CodeDomExpression>();
		}

		public override string ToString()
		{
			switch (ReturnType.Name)
			{
				case "RuntimeTypeHandle":
					return string.Format("typeTokenOf({0})", _tokenProvider);

				case "RuntimeFieldHandle":
					return string.Format("fieldTokenOf({0})", _tokenProvider);

				case "RuntimeMethodHandle":
					return string.Format("methodTokenOf({0})", _tokenProvider);

				default:
					throw new InvalidDataException("Unsupported token type");
			}
		}
	}
}

namespace Mono.Cecil.CodeDom.Parser
{
	using SimpleExpressions;

	public static partial class CodeDom
	{
		public static LoadMetadataTokenExpression LoadMetadataToken(Context context, Instruction position)
		{
			return new LoadMetadataTokenExpression(context, position);
		}
	}
}