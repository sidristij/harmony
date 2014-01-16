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
		long longTestField;

		private void Length_To_Integer_Field_Method()
		{
			var arr = new int[10];
			testField = arr.Length;
		}

		/// <summary>
		/// Basic test for <array>.Length
		/// </summary>
		[Test]
		public void Length_To_Integer_Field()
		{
			var method = TestAssemblyAccessor.ParseMethod(MethodDef.Of(TestAssemblyAccessor.Assembly.MainModule, Length_To_Integer_Field_Method));
			Console.WriteLine(method);
		}

		private void Length_To_Long_Field_Method()
		{
			var arr = new int[10];
			longTestField = arr.LongLength;
		}

		/// <summary>
		/// Basic test for <array>.LongLength 
		/// </summary>
		[Test]
		public void Length_To_Long_Field()
		{
			var method = TestAssemblyAccessor.ParseMethod(MethodDef.Of(TestAssemblyAccessor.Assembly.MainModule, Length_To_Long_Field_Method));
			Console.WriteLine(method);
		}
	}
}

