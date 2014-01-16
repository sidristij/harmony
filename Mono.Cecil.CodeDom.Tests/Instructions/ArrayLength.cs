using System;
using NUnit.Framework;
using System.Reflection;
using Mono.Cecil.CodeDom.Rocks;
using Mono.Cecil.CodeDom.Parser.Arrays;

namespace Mono.Cecil.CodeDom.Tests
{
	[TestFixture]
	public class ArrayLength
	{
		int testField;
		static int staticTestField;

		private void Length_To_Integer_Field_Method()
		{
			var arr = new int[10];
			testField = arr.Length;
		}

		[Test]
		public void Length_To_Integer_Field()
		{
			var module = TestAssemblyAccessor.Assembly.MainModule;
			var method = TestAssemblyAccessor.ParseMethod(MethodDef.Of(module, Length_To_Integer_Field_Method));
			Console.WriteLine(method);
		}

		private void Length_To_Static_Integer_Field_Method()
		{
			var arr = new int[10];
			staticTestField = arr.Length;
		}

		[Test]
		public void Length_To_Static_Integer_Field()
		{
			var module = TestAssemblyAccessor.Assembly.MainModule;
			var method = TestAssemblyAccessor.ParseMethod(MethodDef.Of(module, Length_To_Static_Integer_Field_Method));
			Console.WriteLine(method);
		}
	}
}

