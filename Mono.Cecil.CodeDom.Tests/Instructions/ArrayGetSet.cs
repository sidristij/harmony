using System;
using NUnit.Framework;
using Mono.Cecil.CodeDom.Rocks;

namespace Mono.Cecil.CodeDom.Tests
{
	[TestFixture]
	public class ArrayGetSet
	{
		int intField;

		private void ArrayGetOneDimension_To_Integer_Field_Method()
		{
			var arr = new int[10];
			intField = arr[0];
		}

		/// <summary>
		/// Basic test for <array>.Length
		/// </summary>
		[Test]
		public void ArrayGetOneDimension_To_Integer_Field()
		{
			var method = TestAssemblyAccessor.ParseMethod(MethodDef.Of(TestAssemblyAccessor.Assembly.MainModule, ArrayGetOneDimension_To_Integer_Field_Method));
			Console.WriteLine(method);
		}

		private void ArrayGetTwoDimension_To_Integer_Field_Method()
		{
			var arr = new int[10,10];
			intField = arr[0, 1];
		}

		/// <summary>
		/// Basic test for <array>.Length
		/// </summary>
		[Test]
		public void ArrayGetTwoDimension_To_Integer_Field()
		{
			var method = TestAssemblyAccessor.ParseMethod(MethodDef.Of(TestAssemblyAccessor.Assembly.MainModule, ArrayGetTwoDimension_To_Integer_Field_Method));
			Console.WriteLine(method);
		}
	}
}

