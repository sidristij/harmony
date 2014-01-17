using System;
using Mono.Cecil.Cil;
using System.Reflection;

namespace Mono.Cecil.CodeDom.Rocks.Cil
{
	/// <summary>
	/// Set of methods to check incoming to instructions parameters 
	/// types via ECMA CIL based rules.
	/// </summary>
	public static class Cast
	{
		/*
		 *  ECMA CIL-335, Table III.2.: Binary numeric operations
		 *
		 *  shows the result type for A op B—where op is add, div, mul, rem, or sub—for each
		 *  possible combination of operand types. Boxes holding simply a result type, apply to all five
		 *  instructions. Boxes marked "x" indicate an invalid CIL instruction. Boxes, marked "¡" indicate a CIL
		 *  instruction that is not verifiable. Boxes with a list of instructions are valid only for those
		 *  instructions.
		 *
		 *          |                        B type
		 * -------------------------------------------------------------------------
		 *   A type |      Int32     Int64    IntPtr       F         &         O
		 * -------------------------------------------------------------------------
		 *   Int32  |      Int32       x      IntPtr       x      ¡&(add)      x
		 *
		 *   Int64  |        x       Int64       x         x         x         x
		 *
		 *   IntPtr |      IntPtr      x      IntPtr       x      ¡&(add)      x
		 *
		 *   F      |        x         x         x         F         x         x
		 *
		 *   &      |   ¡&(add,sub)    x    ¡&(add,sub)    x    ¡IntPtr(sub)   x
		 *
		 *   O      |        x         x         x         x         x         x
		 *
		 * */
		public static MetadataType? GetBinaryNumericOpType(OpCode opcode, MetadataType first, MetadataType second)
		{
			switch (first)
			{
				case MetadataType.Int32:
				case MetadataType.IntPtr:
					switch (second)
					{
						case MetadataType.Int32:
						case MetadataType.IntPtr:
							return second;
						case MetadataType.ByReference:
							if (opcode.Code.ToOperationType() == OperationType.Add)
							{
								return second;
							}
							break;
					}
					break;
				case MetadataType.Int64:
					switch (second)
					{
						case MetadataType.Int64:
							return second;
					}
					break;
				case MetadataType.ByReference:
					switch (second)
					{
						case MetadataType.Int32:
						case MetadataType.IntPtr:
							if (opcode.Code.ToOperationType() == OperationType.Add ||
								opcode.Code.ToOperationType() == OperationType.Subtract)
							{
								return first;
							}
							break;
						case MetadataType.ByReference:
							if (opcode.Code.ToOperationType() == OperationType.Subtract)
							{
								return MetadataType.IntPtr;
							}
							break;
					}
					break;
				case MetadataType.Single:
					switch (second)
					{
						case MetadataType.Single:
							return MetadataType.Single;
							break;
					}
			        break;
				case MetadataType.Double:
					switch (second)
					{
						case MetadataType.Double:
							return MetadataType.Double;
							break;
					}
					break;
			}

			return null;
			/*
			switch (first)
			{
				case MetadataType.Int32:
					break;
				case MetadataType.Int64:
					break;
				case MetadataType.IntPtr:
					break;
				case MetadataType.Single: // F
					break;
				case MetadataType.Double:
					break;
				case MetadataType.ByReference:
					break;
				case MetadataType.Object:
					break;
			}
			*/
		}
		/*
		 *  ECMA CIL-335, Table III.3,: Unary Numeric Operations
		 *
		 *  shows the result type for the unary numeric operations. Used for the neg instruction.
		 *  Boxes marked "x" indicate an invalid CIL instruction. All valid uses of this instruction are
		 *  verifiable.
		 *
		 *   Operand type |      Int32     Int64    IntPtr     F      &      O
		 *   Result type  |      Int32     Int64    IntPtr     F      x      x

		 * */
		public static MetadataType? GetUnaryNumericOpType(MetadataType first)
		{
			switch (first)
			{
				case MetadataType.Int32:
				case MetadataType.Int64:
				case MetadataType.IntPtr:
				case MetadataType.Single: // F
				case MetadataType.Double:
					return first;
			}
			return null;
		}
		/*
		 *  ECMA CIL-335, Table III.4.: Binary Comparison or Branch Operations
		 *
		 *  shows the result type for the comparison and branch instructions. The binary
		 *  comparison returns a Boolean value and the branch operations branch based on the top two
		 *  values on the stack. Used for beq, beq.s, bge, bge.s, bge.un, bge.un.s, bgt, bgt.s, bgt.un,
		 *  bgt.un.s, ble, ble.s, ble.un, ble.un.s, blt, blt.s, blt.un, blt.un.s, bne.un, bne.un.s, ceq, cgt,
		 *  cgt.un, clt, clt.un. Boxes marked "√" indicate that all instructions are valid for that © Ecma International 2012 303
		 *  combination of operand types. Boxes marked "x" indicate invalid CIL sequences. Shaded boxes
		 *  boxes indicate a CIL instruction that is not verifiable. Boxes with a list of instructions are valid
		 *  only for those instruction
		 *
		 *          |                        B type
		 * -------------------------------------------------------------------------
		 *   A type |      Int32     Int64    IntPtr     F         &         O
		 * -------------------------------------------------------------------------
		 *   Int32  |        √         x        √        x         x         x
		 *          |
		 *   Int64  |        x         √        x        x         x         x
		 *          |
		 *   IntPtr |        √         x        √        x     beq,bne.un    x
		 *          |                                             ceq
		 *     F    |        x         x        x        √         x         x
		 *          |
		 *     &    |        x         x    beq,bne.un   x        √_1        x
		 *          |                          ceq
		 *     O    |        x         x        x        x         x     beq,bne.un
		 *                                                                 ceq_2
		 *
		 *
		 * 1) Except for beq, bne.un, beq.s, bne.un.s, or ceq these combinations make sense if
		 *    both operands are known to be pointers to elements of the same array. However, t here is
		 *    no security issue for a CLI that does not check this constraint [Note: if the two operands
		 *    are not pointers into the same array, then the result is simply the distance apart in the
		 *    garbage-collected heap of two unrelated data items. This distance apart will almost
		 *    certainly change at the next garbage collection. Essentially, the result cannot be used to
		 *    compute anything useful end note]
		 * 2) cgt.un is allowed and verifiable on ObjectRefs (O). This is commonly used when
		 *    comparing an ObjectRef with null (there is no “compare-not-equal” instruction, which
		 *    would otherwise be a more obvious solution)
		 * */
		public static bool AllowsBinaryComparsion(OpCode opcode, MetadataType first, MetadataType second)
		{
			if (first == MetadataType.Boolean)
				first = MetadataType.Int32;

			if (second == MetadataType.Boolean)
				second = MetadataType.Int32;

			if (first == second)
			{
				if (first == MetadataType.ByReference)
				{
					Console.WriteLine("note 1 isn't checked");
				}
				else 
				if (first == MetadataType.Object)
				{
					if (opcode.Code == Code.Cgt_Un)
					{
						// ECMA CIL-335, Table III.4.: (note 2) cgt.un is allowed and verifiable on ObjectRefs (O).
						return false;
					}
				}
				return true;
			}

			if ((first == MetadataType.IntPtr && second == MetadataType.Int32) ||
				(first == MetadataType.Int32 && second == MetadataType.IntPtr))
			{
				return true;
			}

			if (
				(((first == MetadataType.IntPtr && second == MetadataType.ByReference) ||
				(first == MetadataType.ByReference && second == MetadataType.IntPtr))) &&
				(opcode.Code == Code.Beq || opcode.Code == Code.Bne_Un || opcode.Code == Code.Ceq)
				)
			{
				return true;
			}

			return false;
		}

		/*
		 *  ECMA CIL-335, Table III.5.: Integer Operations
		 *
		 *  shows the result type for each possible combination of operand types in integer
		 *  operations. Used for and, div.un, not, or, rem.un, xor. The div.un and rem.un instructions treat
		 *  their operands as unsigned integers and produce the bit pattern corresponding to the unsigned
		 *  result. As described in the CLI standard, however, the CLI makes no distinction between signed
		 *  and unsigned integers on the stack. The not instruction is unary and returns the same type as the
		 *  input. The shl and shr instructions return the same type as their first operand, and their second
		 *  operand shall be of type int32 or native int. Boxes marked  indicate invalid CIL sequences.
		 *  All other boxes denote verifiable combinations of operands.
		 *
		 *          |                        B type
		 * -------------------------------------------------------------------------
		 *   A type |      Int32     Int64    IntPtr       F         &         O
		 * -------------------------------------------------------------------------
		 *   Int32  |      Int32       x      IntPtr       x         x         x
		 *
		 *   Int64  |        x       Int64       x         x         x         x
		 *
		 *   IntPtr |      IntPtr      x      IntPtr       x         x         x
		 *
		 *     F    |        x         x         x         x         x         x
		 *
		 *     &    |        x         x         x         x         x         x
		 *
		 *     O    |        x         x         x         x         x         x
		 *
		 * */
		public static MetadataType? GetIntegerOpType(MetadataType first, MetadataType second)
		{
			if ((first == second) &&
				(first == MetadataType.Int32 || first == MetadataType.Int64 || first == MetadataType.IntPtr))
			{
				return first;
			}

			if ((first == MetadataType.Int32 && second == MetadataType.IntPtr) ||
				(first == MetadataType.IntPtr && second == MetadataType.Int32))
			{
				return MetadataType.IntPtr;
			}

			return null;
		}
		/*
		 *  ECMA CIL-335, Table III.6.: Shift Operations
		 *
		 *  shows the valid combinations of operands and result for the shift instructions: shl,
		 *  shr, shr.un. Boxes marked "x" indicate invalid CIL sequences. All other boxes denote verifiable
		 *  combinations of operand. If the “Shift-By” operand is larger than the width of the “To-BeShifted”
		 *  operand, then the results are unspecified. (e.g., shift an int32 integer left by 37 bits)
		 *
		 *
		 *                 |                       Shifted by
		 * ------------------------------------------------------------------------------------
		 *   To be shifted |      Int32     Int64    IntPtr       F         &         O
		 * ------------------------------------------------------------------------------------
		 *      Int32      |      Int32       x      Int32        x         x         x
		 *
		 *      Int64      |      Int64       x      Int64        x         x         x
		 *
		 *      IntPtr     |      IntPtr      x      IntPtr       x         x         x
		 *
		 *        F        |        x         x         x         x         x         x
		 *
		 *        &        |        x         x         x         x         x         x
		 *
		 *        O        |        x         x         x         x         x         x
		 *
		 * */
		public static MetadataType? GetShiftOpType(MetadataType first, MetadataType second)
		{
			switch (second)
			{
				case MetadataType.Int32:
				case MetadataType.IntPtr:
					switch (first)
					{
						case MetadataType.Int32:
							return MetadataType.Int32;
						case MetadataType.Int64:
							return MetadataType.Int64;
						case MetadataType.IntPtr:
							return MetadataType.IntPtr;
					}
					break;
			}
			return null;
		}
		/*
		 *  ECMA CIL-335, Table III.7.: Overflow Arithmetic Operations
		 *
		 *  shows the result type for each possible combination of operand types in the arithmetic
		 *  operations with overflow checking. An exception shall be thrown if the result cannot be
		 *  represented in the result type. Used for add.ovf, add.ovf.un, mul.ovf, mul.ovf.un, sub.ovf, and
		 *  sub.ovf.un. For details of the exceptions thrown, see the descriptions of the specific instructions.
		 *  The shaded uses are not verifiable, while boxes marked "x" indicate invalid CIL sequences.
		 *
		 *          |                        B type
		 * -------------------------------------------------------------------------
		 *   A type |      Int32     Int64    IntPtr       F         &         O
		 * -------------------------------------------------------------------------
		 *   Int32  |      Int32       x      IntPtr       x    [&]add.ovf.un  x
		 *
		 *   Int64  |        x       Int64       x         x         x         x
		 *
		 *   IntPtr |      IntPtr      x      IntPtr       x    [&]add.ovf.un  x
		 *
		 *     F    |        x         x         x         x         x         x
		 *
		 *     &    |   [&]add.ovf.un  x   [&]add.ovf.un   x      [IntPtr]     x
		 *               sub.ovf.un         sub.ovf.un           sub.ovf.un
		 *     O    |        x         x         x         x         x         x
		 *
		 * */
		public static MetadataType? CheckOverflowArithmeticOp(OpCode opcode, MetadataType first, MetadataType second)
		{
			switch (first)
			{
				case MetadataType.Int32:
					switch (second)
					{
						case MetadataType.Int32:
						case MetadataType.IntPtr:
							return first;
						case MetadataType.ByReference:
							if (opcode.Code == Code.Add_Ovf_Un)
								return MetadataType.ByReference;
							break;
					}
					break;
				case MetadataType.Int64:
					if (second == MetadataType.Int64)
						return MetadataType.Int64;
					break;
				case MetadataType.IntPtr:
					switch (second)
					{
						case MetadataType.Int32:
						case MetadataType.IntPtr:
							return MetadataType.IntPtr;
						case MetadataType.ByReference:
							if (opcode.Code == Code.Add_Ovf_Un)
								return MetadataType.ByReference;
							break;
					}
					break;
				case MetadataType.ByReference:
					switch (second)
					{
						case MetadataType.Int32:
						case MetadataType.IntPtr:
							if (opcode.Code == Code.Add_Ovf_Un || opcode.Code == Code.Sub_Ovf_Un)
								return MetadataType.ByReference;
							break;
						case MetadataType.ByReference:
							if (opcode.Code == Code.Sub_Ovf_Un)
								return MetadataType.IntPtr;
							break;
					}
					break;
			}
			return null;
		}

		/*
		 *  ECMA CIL-335, Table III.8.: Conversion Operations
		 *
		 *  shows the result type for the conversion operations. Conversion operations convert
		 *  the top item on the evaluation stack from one numeric type to another. While converting,
		 *  truncation or extension occurs as shown in the table. The result type is guaranteed to be
		 *  representable as the data type specified as part of the operation (i.e., the conv.u2 instruction
		 *  returns a value that can be stored in an unsigned int16). The stack, however, can only store
		 *  values that are a minimum of 4 bytes wide. Used for the conv.<to type>, conv.ovf.<to type>,
		 *  and conv.ovf.<to type>.un instructions. The shaded uses are not verifiable, while boxes
		 *  marked "x" indicate invalid CIL sequences.
		 *
		 *                |                              Input (from evaluation stack)
		 * -------------------------------------------------------------------------------------------------------
		 *  Conv-to       |      Int32       Int64       IntPtr            F                &                O
		 * -------------------------------------------------------------------------------------------------------
		 *  int8,uint8    |     Truncate    Truncate    Truncate    Truncate to 0           x                x
		 *  int16, uint16 |
		 *  int32, uint32 |       Nop       Truncate    Truncate    Truncate to 0           x                x
		 *                |
		 *  int64         |    Sign Extend    Nop      Sign Extend  Truncate to 0   Stop GC tracking  Stop GC tracking
		 *                |
		 *  uint64        |    Zero Extend    Nop      Zero Extend  Truncate to 0   Stop GC tracking  Stop GC tracking
		 *                |
		 *  intptr        |    Sign Extend  Truncate       Nop      Truncate to 0   Stop GC tracking  Stop GC tracking
		 *                |
		 *  uintptr       |    Zero Extend  Truncate       Nop      Truncate to 0   Stop GC Tracking  Stop GC tracking
		 *                |
		 *  all floats    |     To Float    To Float    To Float   Change precision         x                x
		 *
		 * 1) “Truncate” means that the number is truncated to the desired size (i.e., the most
		 *    significant bytes of the input value are simply ignored). If the result is narrower than
		 *    the minimum stack width of 4 bytes, then this result is zero extended (if the result type
		 *    is unsigned) or sign-extended (if the result type is signed). Thus, converting the value
		 *    0x1234 ABCD from the evaluation stack to an 8-bit datum yields the result 0xCD; if
		 *    the result type were int8, this is sign-extended to give 0xFFFF FFCD; if, instead, the
		 *    result type were unsigned int8, this is zero-extended to give 0x0000 00CD.
		 *
		 * 2) “Truncate to zero” means that the floating-point number will be converted to an
		 *    integer by truncation toward zero. Thus 1.1 is converted to 1, and –1.1 is converted to –1.
		 *
		 * 3) Converts from the current precision available on the evaluation stack to the precision
		 *    specified by the instruction. If the stack has more precision than the output size the
		 *    conversion is performed using the IEC 60559:1989 “round-to-nearest” mode to compute
		 *    the low order bit of the result.
		 *
		 * 4) “Stop GC Tracking” means that, following the conversion, the item’s value will not be
		 *    reported to subsequent garbage-collection operations (and therefore will not be updated
		 *    by such operations).
		 *
		 * */
		public static bool CheckFromStackTypeConversion(MetadataType first, MetadataType second)
		{
			switch (first)
			{
				case MetadataType.Int32:
				case MetadataType.Int64:
				case MetadataType.IntPtr:
				case MetadataType.Single:
				case MetadataType.Double:
					switch (second)
					{
						case MetadataType.Byte:
						case MetadataType.SByte:
						case MetadataType.Int16:
						case MetadataType.Int32:
						case MetadataType.UInt16:
						case MetadataType.UInt32:
						case MetadataType.Single:
						case MetadataType.Double:
						case MetadataType.IntPtr:
						case MetadataType.UIntPtr:
							return true;
					}
					break;
				case MetadataType.ByReference:
				case MetadataType.Object:
					if (second == MetadataType.IntPtr ||
						second == MetadataType.UIntPtr ||
						second == MetadataType.Int64 ||
						second == MetadataType.UInt64)
					{
						return true;
					}
					break;
			}
			return false;
		}

		/*
		 *  ECMA CIL 335, III.1.1.1 Numeric data types 
		 *  The CLI only operates on the numeric types int32 (4-byte signed integers), int64
		 *  (8-byte signed integers), native int (native-size integers), and F (native-size 
		 *  floating-point numbers). However, the CIL instruction set allows additional data 
		 *  types to be implemented
		 * 
		 * */
		// TODO: need to check information
		public static MetadataType? StackStoringTypeConvertion(MetadataType original)
		{
			switch (original)
			{
				case MetadataType.Boolean:
				case MetadataType.SByte:
				case MetadataType.Char:
				case MetadataType.Int16:
					return MetadataType.Int32;
				case MetadataType.Byte:
				case MetadataType.UInt16:
					return MetadataType.UInt32;
			}
			return original;
		}
	}
}

