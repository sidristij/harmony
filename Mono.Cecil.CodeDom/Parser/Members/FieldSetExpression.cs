using System;
using Mono.Cecil.Cil;
using Mono.Cecil.CodeDom.Collections;
using Mono.Cecil.CodeDom.Rocks;

namespace Mono.Cecil.CodeDom.Parser.Members
{
	public sealed class FieldSetExpression : CodeDomSingleInstructionExpression, ICodeDomStaticInstance
	{
		public const int InstancePos = 0;
		public const int ValuePos = 1;
		public const int MaxNodes = 2;

		public FieldSetExpression(Context context, Instruction position, FieldReference ref_field, CodeDomExpression exp_value)
			: this(context, position, ref_field, exp_value, new CodeDomExpression(context))
		{
		}

		public FieldSetExpression(Context context, Instruction position, FieldReference ref_field, CodeDomExpression exp_value, CodeDomExpression exp_instance)
			: base(context, position)
		{
			if (!exp_instance.ReturnType.HardEquals(ref_field.DeclaringType))
			{
				throw new InvalidOperationException(string.Format("instance field '{0}' is not member of '{1}' type", ref_field.FullName, exp_instance.ReturnType.FullName));
			}

			if(!ref_field.FieldType.HardEquals(exp_value.ReturnType))
			{
				throw new InvalidOperationException(string.Format("instance field type '{0}' is not equals to value type '{1}'", ref_field.FieldType.FullName, exp_value.ReturnType.FullName));
			}

			// instance exp
			InstanceExpression = exp_instance;

			// base class
			ReadsStack = InstanceExpression.IsEmpty ? 1 : 2;
			WritesStack = 0;
			ReturnType = Context.MakeRef(typeof(void));
			Nodes = new FixedList<CodeDomExpression>(MaxNodes);

			// this
			FieldReference = ref_field;
			ValueExpression = exp_value;
		}

		public FieldReference FieldReference { get; private set; }

		public CodeDomExpression InstanceExpression { get { return Nodes[InstancePos]; } set { Nodes[InstancePos] = value; value.ParentNode = this; } }

		public CodeDomExpression ValueExpression { get { return Nodes[ValuePos]; } set { Nodes[ValuePos] = value; value.ParentNode = this; } }

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
			return string.Format("{0}.{1} = {2}{3}", InstanceExpression, FieldReference.Name, ValueExpression, FinalString);
		}
	}
}

namespace Mono.Cecil.CodeDom.Parser
{
	using Members;

	public static partial class CodeDom
	{
		public static FieldSetExpression FieldSet(Context context, Instruction position, FieldReference ref_field, CodeDomExpression exp_value, CodeDomExpression exp_instance = null)
		{
			return new FieldSetExpression(context, position, ref_field, exp_value, exp_instance);
		}
	}
}

