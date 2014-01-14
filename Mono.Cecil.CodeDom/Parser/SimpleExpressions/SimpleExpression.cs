using System;
using Mono.Cecil.Cil;
using Mono.Cecil.CodeDom.Collections;
using Mono.Cecil.CodeDom.Rocks.Cil;

namespace Mono.Cecil.CodeDom.Parser.SimpleExpressions
{
	public sealed class SimpleExpression : CodeDomSingleInstructionExpression
	{
		public const int LeftPos = 0;
		public const int RightPos = 1;
		public const int MaxNodes = 2;

		public SimpleExpression(Context context, Instruction position, CodeDomExpression exp_left, CodeDomExpression exp_right)
			: base(context, position)
		{
			if (exp_left.WritesStack != 1 && exp_right.WritesStack != 1)
			{
				throw new ArgumentOutOfRangeException(string.Format("SimpleExpression {0} requres two Expressions in the stack and each expression should to push to stack 1 value.", position.OpCode.Code));
			}

			// if type is not resolved, incoming args error
			if (ReturnType == null)
			{
				var left_type = exp_left.ReturnType;
				var right_type = exp_right.ReturnType;
				throw new ArgumentException(string.Format("Error in {0} operator: incoming types are unsupported ({1}, {2})", position.OpCode.Code, left_type, right_type));
			}

			// base class
			ReadsStack = 2;
			WritesStack = 1;
			Nodes = new FixedList<CodeDomExpression>(MaxNodes);

			// this
			Left = exp_left;
			Right = exp_right;
			Operation = position.OpCode.Code.ToOperationType();
			OverflowCheckType = position.OpCode.Code.ToOverflowCheck();
			IsUnsigned = position.OpCode.Code.IsUnsigned();

			MetadataType? metadataType =  null;
			switch (Operation)
			{
				case OperationType.LogicalNeg:
				case OperationType.LogicalAnd:
				case OperationType.LogicalOr:
				case OperationType.LogicalXor:
					metadataType = Cast.GetIntegerOpType(Left.ReturnType.MetadataType, Right.ReturnType.MetadataType);
					break;
				case OperationType.Equal:
				case OperationType.NotEqual:
				case OperationType.Greater:
				case OperationType.Less:
				case OperationType.GreaterOrEqual:
				case OperationType.LessOrEqual:
					if (Cast.AllowsBinaryComparsion(position.OpCode, Left.ReturnType.MetadataType, Right.ReturnType.MetadataType))
					{
						metadataType = MetadataType.Boolean;
					}
					break;
				case OperationType.Add:
				case OperationType.Subtract:
				case OperationType.Divide:
				case OperationType.Remainder:
				case OperationType.Multiply:
					metadataType = Cast.GetBinaryNumericOpType(position.OpCode, Left.ReturnType.MetadataType, Right.ReturnType.MetadataType);
					break;
				case OperationType.ShiftLeft:
				case OperationType.ShiftRight:
					metadataType = Cast.GetShiftOpType(Left.ReturnType.MetadataType, Right.ReturnType.MetadataType);
					break;
			}

			// If type isn't found, error found
			if (!metadataType.HasValue)
			{
				throw new ArgumentException(string.Format("Using types {0} and {1} isn't acceptable in binary operations", Left, Right));
			} 

			// Return type resolved
			ReturnType = context.Module.Import(metadataType.Value);		
		}

		public CodeDomExpression Left
		{
			get { return Nodes[LeftPos]; }
			set { Nodes[LeftPos] = value;
				value.ParentNode = this; }
		}

		public CodeDomExpression Right
		{
			get { return Nodes[RightPos]; }
			set { Nodes[RightPos] = value;
				value.ParentNode = this; }
		}

		public OperationType Operation { get; private set; }

		public OverflowCheckType OverflowCheckType { get; private set; }

		public bool IsUnsigned { get; private set; }

		public override string ToString()
		{
			string op = null;

			switch (this.Operation)
			{
				case OperationType.LogicalNeg:     op = "!"; break;
				case OperationType.LogicalAnd:     op = "&"; break;
				case OperationType.LogicalOr:      op = "|"; break;
				case OperationType.LogicalXor:     op = "^"; break;
				case OperationType.Equal:          op = "=="; break;
				case OperationType.NotEqual:       op = "!="; break;
				case OperationType.Greater:        op = ">"; break;
				case OperationType.Less:           op = "<"; break;
				case OperationType.GreaterOrEqual: op = ">="; break;
				case OperationType.LessOrEqual:    op = "<="; break;
				case OperationType.Add:            op = "+"; break;
				case OperationType.Subtract:       op = "-"; break;
				case OperationType.Divide:         op = "/"; break;
				case OperationType.Remainder:      op = "%"; break;
				case OperationType.Multiply:       op = "*"; break;
				case OperationType.ShiftLeft:      op = "<<"; break;
				case OperationType.ShiftRight:     op = ">>"; break;
				default:
					op = Operation.ToString();
					break;
			}
			return string.Format("{0} {2} {1}", Left, Right, op);
		}
	}
}
namespace Mono.Cecil.CodeDom.Parser
{
	using SimpleExpressions;

	public static partial class CodeDom
	{
		public static SimpleExpression SimpleExpression(Context context, Instruction position, CodeDomExpression exp_left, CodeDomExpression exp_right)
		{
			return new SimpleExpression(context, position, exp_left, exp_right);
		}
	}
}

