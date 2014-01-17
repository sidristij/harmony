using System;
using NUnit.Framework;
using Mono.Cecil.CodeDom.Rocks;

// ReSharper disable InconsistentNaming
namespace Mono.Cecil.CodeDom.Tests
{
	[TestFixture]
	public class ArrayGetSet
	{
		private int intField;
		private static int staticIntField;

		#region instance, from field (get)

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

		#endregion 

		#region instance, to field (set)

		private void ArrayGetOneDimension_From_Integer_Field_Method()
		{
			var arr = new int[10];
			arr[0] = intField;
		}

		/// <summary>
		/// Basic test for <array>.Length
		/// </summary>
		[Test]
		public void ArrayGetOneDimension_From_Integer_Field()
		{
			var method = TestAssemblyAccessor.ParseMethod(MethodDef.Of(TestAssemblyAccessor.Assembly.MainModule, ArrayGetOneDimension_From_Integer_Field_Method));
			Console.WriteLine(method);
		}

		private void ArrayGetTwoDimension_From_Integer_Field_Method()
		{
			var arr = new int[10,10];
			arr[0, 1] = intField;
		}

		/// <summary>
		/// Basic test for <array>.Length
		/// </summary>
		[Test]
		public void ArrayGetTwoDimension_From_Integer_Field()
		{
			var method = TestAssemblyAccessor.ParseMethod(MethodDef.Of(TestAssemblyAccessor.Assembly.MainModule, ArrayGetTwoDimension_From_Integer_Field_Method));
			Console.WriteLine(method);
		}

		#endregion

		#region static, from field (get)

		private void ArrayGetOneDimension_Static_To_Integer_Field_Method()
		{
			var arr = new int[10];
			staticIntField = arr[0];
		}

		/// <summary>
		/// Basic test for <array>.Length
		/// </summary>
		[Test]
		public void ArrayGetOneDimension_Static_To_Integer_Field()
		{
			var method = TestAssemblyAccessor.ParseMethod(MethodDef.Of(TestAssemblyAccessor.Assembly.MainModule, ArrayGetOneDimension_Static_To_Integer_Field_Method));
			Console.WriteLine(method);
		}

		private void ArrayGetTwoDimension_Static_To_Integer_Field_Method()
		{
			var arr = new int[10, 10];
			staticIntField = arr[0, 1];
		}

		/// <summary>
		/// Basic test for <array>.Length
		/// </summary>
		[Test]
		public void ArrayGetTwoDimension_Static_To_Integer_Field()
		{
			var method = TestAssemblyAccessor.ParseMethod(MethodDef.Of(TestAssemblyAccessor.Assembly.MainModule, ArrayGetTwoDimension_Static_To_Integer_Field_Method));
			Console.WriteLine(method);
		}

		#endregion

		#region static, to field (set)

		private void ArrayGetOneDimension_Static_From_Integer_Field_Method()
		{
			var arr = new int[10];
			arr[0] = staticIntField;
		}

		/// <summary>
		/// Basic test for <array>.Length
		/// </summary>
		[Test]
		public void ArrayGetOneDimension_Static_From_Integer_Field()
		{
			var method = TestAssemblyAccessor.ParseMethod(MethodDef.Of(TestAssemblyAccessor.Assembly.MainModule, ArrayGetOneDimension_Static_From_Integer_Field_Method));
			Console.WriteLine(method);
		}

		private void ArrayGetTwoDimension_Static_From_Integer_Field_Method()
		{
			var arr = new int[10, 10];
			arr[0, 1] = staticIntField;
		}

		/// <summary>
		/// Basic test for <array>.Length
		/// </summary>
		[Test]
		public void ArrayGetTwoDimension_Static_From_Integer_Field()
		{
			var method = TestAssemblyAccessor.ParseMethod(MethodDef.Of(TestAssemblyAccessor.Assembly.MainModule, ArrayGetTwoDimension_Static_From_Integer_Field_Method));
			Console.WriteLine(method);
		}

		#endregion
	}
}
// ReSharper restore InconsistentNaming