using System;
using Mono.Cecil.Cil;

namespace Mono.Cecil.CodeDom
{
	internal static class OpCodesEx
	{
		public static OperationType ToOperationType(this Code opcodeValue)
		{
			switch (opcodeValue)
			{
				case Code.Beq:
					return OperationType.Equal;
				case Code.Bge:
				case Code.Bge_Un:
					return OperationType.GreaterOrEqual;
				case Code.Bgt:
				case Code.Bgt_Un:
					return OperationType.Greater;
				case Code.Ble:
				case Code.Ble_Un:
					return OperationType.LessOrEqual;
				case Code.Blt:
				case Code.Blt_Un:
					return OperationType.Less;
				case Code.Bne_Un:
					return OperationType.NotEqual;
				case Code.Add:
				case Code.Add_Ovf:
				case Code.Add_Ovf_Un:
					return OperationType.Add;
				case Code.Sub:
				case Code.Sub_Ovf:
				case Code.Sub_Ovf_Un:
					return OperationType.Subtract;
				case Code.Mul:
				case Code.Mul_Ovf:
				case Code.Mul_Ovf_Un:
					return OperationType.Multiply;
				case Code.Div:
				case Code.Div_Un:
					return OperationType.Divide;
				case Code.Rem:
				case Code.Rem_Un:
					return OperationType.Remainder;
				case Code.And:
					return OperationType.LogicalAnd;
				case Code.Or:
					return OperationType.LogicalOr;
				case Code.Xor:
					return OperationType.LogicalXor;
				case Code.Shl:
					return OperationType.ShiftLeft;
				case Code.Shr:
				case Code.Shr_Un:
					return OperationType.ShiftRight;
				case Code.Neg:
					return OperationType.UnaryMinus;
				case Code.Not:
					return OperationType.Not;
				case Code.Ceq:
					return OperationType.Equal;
				case Code.Cgt:
				case Code.Cgt_Un:
					return OperationType.Greater;
				case Code.Clt:
				case Code.Clt_Un:
					return OperationType.Less;
				default:
					throw new ArgumentException(string.Format("Unexpected opcode: {0}.", opcodeValue));
			}
		}

		public static OverflowCheckType ToOverflowCheck(this Code opcodeValue)
		{
			switch (opcodeValue)
			{
				case Code.Conv_Ovf_I1_Un:
				case Code.Conv_Ovf_I2_Un:
				case Code.Conv_Ovf_I4_Un:
				case Code.Conv_Ovf_I8_Un:
				case Code.Conv_Ovf_U1_Un:
				case Code.Conv_Ovf_U2_Un:
				case Code.Conv_Ovf_U4_Un:
				case Code.Conv_Ovf_U8_Un:
				case Code.Conv_Ovf_I_Un:
				case Code.Conv_Ovf_U_Un:
				case Code.Conv_Ovf_I1:
				case Code.Conv_Ovf_U1:
				case Code.Conv_Ovf_I2:
				case Code.Conv_Ovf_U2:
				case Code.Conv_Ovf_I4:
				case Code.Conv_Ovf_U4:
				case Code.Conv_Ovf_I8:
				case Code.Conv_Ovf_U8:
				case Code.Conv_Ovf_I:
				case Code.Conv_Ovf_U:
				case Code.Add_Ovf:
				case Code.Add_Ovf_Un:
				case Code.Mul_Ovf:
				case Code.Mul_Ovf_Un:
				case Code.Sub_Ovf:
				case Code.Sub_Ovf_Un:
					return OverflowCheckType.Enabled;
				default:
					return OverflowCheckType.Disabled;
			}
		}

		public static bool IsUnsigned(this Code opcodeValue)
		{
			switch (opcodeValue)
			{
				case Code.Cgt_Un:
				case Code.Clt_Un:
	
				case Code.Bne_Un:
				case Code.Bge_Un:
				case Code.Bgt_Un:
				case Code.Ble_Un:
				case Code.Blt_Un:
				case Code.Bne_Un_S:
				case Code.Bge_Un_S:
				case Code.Bgt_Un_S:
				case Code.Ble_Un_S:
				case Code.Blt_Un_S:
				
				case Code.Div_Un:
				case Code.Rem_Un:
				case Code.Shr_Un:
				case Code.Add_Ovf_Un:
				case Code.Mul_Ovf_Un:
				case Code.Sub_Ovf_Un:
				
				case Code.Conv_R_Un:
				case Code.Conv_Ovf_I1_Un:
				case Code.Conv_Ovf_I2_Un:
				case Code.Conv_Ovf_I4_Un:
				case Code.Conv_Ovf_I8_Un:
				case Code.Conv_Ovf_U1_Un:
				case Code.Conv_Ovf_U2_Un:
				case Code.Conv_Ovf_U4_Un:
				case Code.Conv_Ovf_U8_Un:
				case Code.Conv_Ovf_I_Un:
				case Code.Conv_Ovf_U_Un:
					return true;
				default:
					return false;
			}
		}

		public static OpCode ToOpCode(this OperationType optype, bool isUnsigned = false, OverflowCheckType overflowCheckType = OverflowCheckType.Disabled)
		{
			if (overflowCheckType == OverflowCheckType.Enabled)
			{
				/*
					case Code.Conv_Ovf_I1_Un:
					case Code.Conv_Ovf_I2_Un:
					case Code.Conv_Ovf_I4_Un:
					case Code.Conv_Ovf_I8_Un:
					case Code.Conv_Ovf_U1_Un:
					case Code.Conv_Ovf_U2_Un:
					case Code.Conv_Ovf_U4_Un:
					case Code.Conv_Ovf_U8_Un:
					case Code.Conv_Ovf_I_Un:
					case Code.Conv_Ovf_U_Un:
					case Code.Conv_Ovf_I1:
					case Code.Conv_Ovf_U1:
					case Code.Conv_Ovf_I2:
					case Code.Conv_Ovf_U2:
					case Code.Conv_Ovf_I4:
					case Code.Conv_Ovf_U4:
					case Code.Conv_Ovf_I8:
					case Code.Conv_Ovf_U8:
					case Code.Conv_Ovf_I:
					case Code.Conv_Ovf_U:
				*/
				if (isUnsigned)
				{
					switch (optype)
					{
						case OperationType.Add:
							return OpCodes.Add_Ovf_Un;
						case OperationType.Multiply:
							return OpCodes.Mul_Ovf_Un;
						case OperationType.Subtract:
							return OpCodes.Mul_Ovf_Un;
					}
				}
				else
				{
					switch (optype)
					{
						case OperationType.Add:
							return OpCodes.Add_Ovf;
						case OperationType.Multiply:
							return OpCodes.Mul_Ovf;
						case OperationType.Subtract:
							return OpCodes.Mul_Ovf;
					}
				}
			}
			else
			{
				switch (optype)
				{
					case OperationType.Equal:
						return OpCodes.Beq;

					case OperationType.BranchWhenEqual:
						return OpCodes.Ceq;

					case OperationType.GreaterOrEqual:
						return isUnsigned ? OpCodes.Bge_Un : OpCodes.Bge;

					case OperationType.Greater:
						return isUnsigned ? OpCodes.Bgt_Un : OpCodes.Bgt;

					case OperationType.BranchWhenGreater:
						return isUnsigned ? OpCodes.Cgt_Un : OpCodes.Cgt;

					case OperationType.LessOrEqual:
						return isUnsigned ? OpCodes.Ble_Un : OpCodes.Ble;

					case OperationType.Less:
						return isUnsigned ? OpCodes.Blt_Un : OpCodes.Blt;

					case OperationType.BranchWhenLess:
						return isUnsigned ? OpCodes.Clt_Un : OpCodes.Clt;

					case OperationType.NotEqual:
						return OpCodes.Bne_Un;
						
					case OperationType.Add:
						return OpCodes.Add;
				
					case OperationType.Subtract:
						return OpCodes.Sub;

					case OperationType.Multiply:
						return OpCodes.Mul;

					case OperationType.Divide:
						return OpCodes.Div;

					case OperationType.Remainder:
						return isUnsigned ? OpCodes.Rem_Un : OpCodes.Rem;

					case OperationType.LogicalOr:
						return OpCodes.Or;

					case OperationType.LogicalXor:
						return OpCodes.Xor;

					case OperationType.LogicalAnd:
						return OpCodes.And;

					case OperationType.ShiftLeft:
						return OpCodes.Shl;

					case OperationType.ShiftRight:
						return isUnsigned ? OpCodes.Shr_Un : OpCodes.Shr;
					
					case OperationType.Not:
						return OpCodes.Not;

					case OperationType.UnaryMinus:
						return OpCodes.Neg;
				}
			}
			throw new ArgumentException("Arguments contains wrong instruction");
		}
	}
}

