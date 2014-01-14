using System;

namespace Mono.Cecil.CodeDom.Parser.SimpleExpressions
{
	[Flags]
	public enum Op
	{
		Unsigned = 0x00000001,
		Overflow = 0x00000002,

		Ceq = 0x10,
		Cgt,
		Clt,

		And,
		Or,
		Xor,

		Add,
		Sub,
		Mul,
		Div,
		Rem,

		Shl,
		Shr
	}
}

