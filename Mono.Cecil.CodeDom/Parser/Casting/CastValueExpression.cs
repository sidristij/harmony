using System;
using Mono.Cecil.Cil;
using Mono.Cecil.CodeDom.Collections;
using Mono.Cecil.CodeDom.Rocks;

namespace Mono.Cecil.CodeDom.Parser.Casting
{
	public sealed class CastValueExpression : CodeDomCastingExpression
	{
		public const int InstancePos = 0;
		public const int MaxNodes = 1;

		public CastValueExpression(Context context, Instruction position, CodeDomExpression exp_instance)
			: base(context, position)
		{
			TypeReference ref_typeto;

			CheckPrimitive(exp_instance.ReturnType);

			switch(position.OpCode.Code)
			{
				case Code.Conv_I:
				case Code.Conv_Ovf_I:
				case Code.Conv_Ovf_I_Un:
					ref_typeto = Context.MakeRef<IntPtr>();
					break;

				case Code.Conv_I1:
				case Code.Conv_Ovf_I1:
				case Code.Conv_Ovf_I1_Un:
					ref_typeto = Context.MakeRef<SByte>();
					break;

				case Code.Conv_I2:
				case Code.Conv_Ovf_I2:
				case Code.Conv_Ovf_I2_Un:
					ref_typeto = Context.MakeRef<Int16>();
					break;

				case Code.Conv_I4:
				case Code.Conv_Ovf_I4:
				case Code.Conv_Ovf_I4_Un:
					ref_typeto = Context.MakeRef<Int32>();
					break;

				case Code.Conv_I8:
				case Code.Conv_Ovf_I8:
				case Code.Conv_Ovf_I8_Un:
					ref_typeto = Context.MakeRef<Int64>();
					break;

				case Code.Conv_U:
				case Code.Conv_Ovf_U:
				case Code.Conv_Ovf_U_Un:
					ref_typeto = Context.MakeRef<UIntPtr>();
					break;

				case Code.Conv_U1:
				case Code.Conv_Ovf_U1:
				case Code.Conv_Ovf_U1_Un:
					ref_typeto = Context.MakeRef<Byte>();
					break;

				case Code.Conv_U2:
				case Code.Conv_Ovf_U2:
				case Code.Conv_Ovf_U2_Un:
					ref_typeto = Context.MakeRef<UInt16>();
					break;

				case Code.Conv_U4:
				case Code.Conv_Ovf_U4:
				case Code.Conv_Ovf_U4_Un:
					ref_typeto = Context.MakeRef<UInt32>();
					break;

				case Code.Conv_U8:
				case Code.Conv_Ovf_U8:
				case Code.Conv_Ovf_U8_Un:
					ref_typeto = Context.MakeRef<UInt64>();
					break;

				case Code.Conv_R_Un:
					ref_typeto = Context.MakeRef<Single>();
					break;

				case Code.Conv_R4:
					ref_typeto = Context.MakeRef<Single>();
					break;

				case Code.Conv_R8:
					ref_typeto = Context.MakeRef<Double>();
					break;

				default:
					throw new ArgumentException(string.Format("Instruction operand have unsupported type to cast to: {0} opcode code", position.OpCode.Code));
			}

			if(exp_instance.WritesStack != 1)
			{
				throw new InvalidOperationException(string.Format("Wrong stack items count in <exp_length> argument: {0}", exp_instance.WritesStack));
			}

			if(!Cast.IsAvaliable(exp_instance.ReturnType, ref_typeto))
			{
				throw new InvalidCastException(string.Format("<exp_instance> ({0}) type cannot be initialized as <ref_type> ({1})", exp_instance, ref_typeto));
			}

			// base class
			ReadsStack = 1;
			WritesStack = 1;
			Nodes = new FixedList<CodeDomExpression>(MaxNodes);
			ReturnType = ref_typeto;

			// this
			InstanceExpression = exp_instance;
			CastTo = ref_typeto;
		}

		void CheckPrimitive(TypeReference returnType)
		{
			if(!returnType.IsPrimitive)
			{
				throw new ArgumentException(string.Format("type should be primitive ({0})", returnType));
			}
		}

		void CheckType<TType>(TypeReference returnType)
		{
			if(!returnType.HardEquals(Context.MakeRef<TType>()))
			{
				throw new ArgumentException(string.Format("type casting from {0} to {1} is wrong)", returnType, typeof(TType)));
			}
		}

		public CodeDomExpression InstanceExpression  { get { return Nodes[InstancePos]; } set { Nodes[InstancePos] = value;  value.ParentNode = this; } }

		public override string ToString()
		{
			return string.Format("({1})({0})", InstanceExpression, ReturnType);
		}
	}
}

namespace Mono.Cecil.CodeDom.Parser
{
	using Casting;

	public static partial class CodeDom
	{
		public static CastValueExpression CastValue(Context context, Instruction position, CodeDomExpression exp_instance)
		{
			return new CastValueExpression(context, position, exp_instance);
		}
	}
}