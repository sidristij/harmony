using System;

namespace Mono.Cecil.CodeDom
{
	public enum OperationType
	{
		Not,
		UnaryPlus,
		UnaryMinus,
		LogicalNeg,
		LogicalAnd,
		LogicalOr,
		LogicalXor,
		
		BranchWhenLess,
		BranchWhenEqual,
		BranchWhenGreater,
		
		Less,
		Equal,
		NotEqual,
		Greater,
		GreaterOrEqual,
		LessOrEqual,

		Add,
		Subtract,
		Divide,
		Multiply,
		Remainder,

		ShiftLeft,
		ShiftRight,
		/*
		PrefixIncrement,
		PrsfixDecrement,
		PostfixIncrement,
		PostfixDecrement,
		Assignment,
		AssignmentAdd,
		AssignmentSubtract,
		AssignmentMultiply,
		AssignmentDivide,
		AssignmentRemainder,
		AssignmentLogicalAnd,
		AssignmentLogicalOr,
		AssignmentLogicalXor,
		AssignmentShiftLeft,
		AssignmentShiftRight,
		ConditionalAnd,
		ConditionalOr,
		NullCoalescing
		*/
	}
}

