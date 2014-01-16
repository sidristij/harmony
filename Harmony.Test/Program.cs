using System;
using Mono.Cecil;
using Mono.Cecil.Rocks;
using Mono.Cecil.CodeDom.Rocks;
using Mono.Cecil.CodeDom.Parser;
using System.Collections.Generic;
using Mono.Cecil.CodeDom;
using System.Linq;
using Mono.Cecil.CodeDom.Parser.Tcf;

namespace Harmony.Test
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			Console.ReadKey();
		}

		private static void TestMethod(int integer, object simpleObject)
		{
			try {
				if (simpleObject != null)
				{
					Console.WriteLine("Hello, {0} and {1}", integer-10+5*integer/75, simpleObject);
				}
			}
			finally
			{
				while (integer - 6 > 0)
				{
					Console.WriteLine("in loop");
				}
			}
		}
	}
}
