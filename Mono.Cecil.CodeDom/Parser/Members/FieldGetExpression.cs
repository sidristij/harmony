using System;
using Mono.Cecil.Cil;
using Mono.Cecil.CodeDom.Collections;
using Mono.Cecil.CodeDom.Rocks;

namespace Mono.Cecil.CodeDom.Parser.Members
{
	public sealed class FieldGetExpression : CodeDomSingleInstructionExpression, ICodeDomStaticInstance
	{
		public const int InstancePos = 0;
		public const int MaxNodes = 1;

		public FieldGetExpression(Context context, Instruction position, FieldReference ref_field)
			: this(context, position, ref_field, new CodeDomExpression(context))
		{
		}

		public FieldGetExpression(Context context, Instruction position, FieldReference ref_field, CodeDomExpression exp_instance)
			: base(context, position)
		{
			if(!exp_instance.ReturnType.HardEquals(ref_field.DeclaringType))
			{
				throw new InvalidOperationException(string.Format("instance field '{0}' is not member of '{1}' type", ref_field.FullName, exp_instance.ReturnType.FullName));
			}

			Nodes = new FixedList<CodeDomExpression>(MaxNodes);

			// instance exp
			InstanceExpression = exp_instance;

			// base class
			ReturnType = ref_field.FieldType;
			ReadsStack = InstanceExpression.IsEmpty ? 0 : 1;
			WritesStack = 1;

			// this
			FieldReference = ref_field;
		}

		public CodeDomExpression InstanceExpression { get { return Nodes[InstancePos]; } set { Nodes[InstancePos] = value; value.ParentNode = this; } }

		public FieldReference FieldReference { get; private set; }

		#region ICodeDomStaticInstance implementation

		public bool IsStatic 
		{ 
			get {
				return InstanceExpression.IsEmpty;
			}
		}

		#endregion

		public override string ToString()
		{
			return string.Format("{0}.{1}", InstanceExpression, FieldReference.Name);
		}
	}
}

namespace Mono.Cecil.CodeDom.Parser 
{
	using Members;

	public static partial class CodeDom
	{
		public static FieldGetExpression FieldGet(Context context, Instruction position, FieldReference ref_field, CodeDomExpression exp_instance = null)
		{
			return new FieldGetExpression(context, position, ref_field, exp_instance);
		}
	}
}

