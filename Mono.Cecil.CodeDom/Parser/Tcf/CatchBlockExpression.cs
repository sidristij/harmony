using Mono.Cecil.Cil;
using Mono.Cecil.CodeDom.Collections;

namespace Mono.Cecil.CodeDom.Parser.Tcf
{

	public sealed class CatchBlockExpression : CodeDomExpression
	{
		public const int BodyPos = 0;
		private const int MaxNodes = 1;

		private readonly IFixedList<CodeDomExpression> _handlersContainer = new FixedList<CodeDomExpression>(MaxNodes);

		public CatchBlockExpression(Context context, TypeReference ref_catchType, CodeDomExpression exp_catch) : base(context)
		{
			Nodes = _handlersContainer;
			Test = ref_catchType;
			Body = exp_catch;
		}

		public CatchBlockExpression(Context context, TypeReference ref_exception) : base(context)
		{
			Test = ref_exception;
			Body = new CodeDomGroupExpression(context);
			Nodes = _handlersContainer;
			Body.ParentNode = this;
		}

		public TypeReference Test { get; private set; }

		public VariableReference VariableReference { get; set; }

		public CodeDomExpression Body
		{
			get { return _handlersContainer[BodyPos]; }
			private set { _handlersContainer[BodyPos] = value; value.ParentNode = this; }
		}

		public override string ToString()
		{
			return string.Format("catch({0}{1}){{ {2} }}", Test, VariableReference == null ? "" : " " + VariableReference, Body);
		}
	}
}
