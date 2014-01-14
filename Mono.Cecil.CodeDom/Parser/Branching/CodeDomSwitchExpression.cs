using System.Text;
using System.Threading;
using Mono.Cecil.Cil;
using Mono.Cecil.CodeDom.Collections;

namespace Mono.Cecil.CodeDom.Parser.Branching
{
	public sealed class CodeDomSwitchExpression : CodeDomSingleInstructionExpression
	{
		public const int IndexPos = 0;
		public const int DefaultPos = 1;
		public const int MaxNodes = 2;

		public CodeDomSwitchExpression(Context context, Instruction target, CodeDomExpression exp_index, CodeDomExpression exp_default, params CodeDomCaseExpression[] exp_cases)
			: base(context, target)
		{
			var merged = new MergedList<CodeDomExpression>();
			merged.AddList(new FixedList<CodeDomExpression>(MaxNodes));
			merged.AddList(new FixedListByDelegate<CodeDomExpression>(i => Cases[i], (i, expression) => Cases[i] = (CodeDomCaseExpression)expression, () => Cases.Length));
			Nodes = merged;
			Cases = exp_cases;
			Index = exp_index;
			Default = exp_default;
		}

		public CodeDomExpression Index  { get { return Nodes[IndexPos]; } set { Nodes[IndexPos] = value; value.ParentNode = this; } }

		public CodeDomExpression Default { get { return Nodes[DefaultPos]; } set { Nodes[DefaultPos] = value; value.ParentNode = this; } }

		public CodeDomCaseExpression[] Cases { get; private set; }

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendFormat("switch({0}) {{", Index);
			sb.AppendLine();
			foreach (var caseExpression in Cases)
			{
				foreach (var val in caseExpression.Values)
				{
					sb.AppendFormat("    case {0}:", val);
					sb.AppendLine();
				}
				sb.AppendFormat("        {0}", caseExpression.Body);
				sb.AppendLine();
				sb.AppendLine("         break;");
			}

			if (!Default.IsEmpty)
			{
				sb.AppendFormat("    default:");
				sb.AppendLine();
				sb.AppendFormat("        {0}", Default);
				sb.AppendLine();
			}
			sb.AppendLine("}");
			return sb.ToString();
		}
	}
}
