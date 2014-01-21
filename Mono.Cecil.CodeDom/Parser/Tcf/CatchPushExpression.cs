using Mono.Cecil.CodeDom.Collections;

namespace Mono.Cecil.CodeDom.Parser.Tcf
{
	/// <summary>
	/// Should be used to emulate pushing exception object to stack by SEH, because this is out of flow.  
	/// </summary>
	public sealed class CatchPushExpression : CodeDomExpression
	{
		public CatchPushExpression(Context context, TypeReference ref_catchType) : base(context)
		{
			// base class
			ReturnType = ref_catchType;
			WritesStack = 1;
			Nodes = new FixedList<CodeDomExpression>();
		}

		/// <inheritdoc/>
		public override bool IsParsed
		{
			get { return true; }
		}

		/// <inheritdoc/>
		public override bool IsEmpty
		{
			get { return false; }
		}

		public override string ToString()
		{
			return string.Format("[CatchPushExpression]");
		}
	}
}

namespace Mono.Cecil.CodeDom.Parser
{
	using Tcf;

	public static partial class CodeDom
	{
		public static CatchPushExpression CatchPush(Context context, TypeReference ref_catchType)
		{
			return new CatchPushExpression(context, ref_catchType);
		}
	}
}