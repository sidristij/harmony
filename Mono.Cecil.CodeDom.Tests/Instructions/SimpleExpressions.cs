using System;
using Mono.Cecil.CodeDom.Rocks;
using NUnit.Framework;

namespace Mono.Cecil.CodeDom.Tests.Instructions
{
	[TestFixture]
	public class SimpleExpressions
	{
		#region Neg/Not/Push
				
		[Test]
		public void Neg()
		{
			Console.WriteLine(
				TestAssemblyAccessor.ParseMethod(
					MethodDef.Of(TestAssemblyAccessor.Assembly.MainModule, 
						delegate {
							int x = 10;
							int y = -x;
						})
				)
			);
		}

		[Test]
		public void Not()
		{
			Console.WriteLine(
				TestAssemblyAccessor.ParseMethod(
				MethodDef.Of(TestAssemblyAccessor.Assembly.MainModule, 
					delegate {
						int x = 0xf0f0;
						Console.WriteLine(~x);
					})
				)
			);
		}

		[Test]
		public void Xor()
		{
			Console.WriteLine(
				TestAssemblyAccessor.ParseMethod(
				MethodDef.Of(TestAssemblyAccessor.Assembly.MainModule, 
					delegate {
						int x = 0xf0f0;
						Console.WriteLine(x ^ 0xffff);
					})
				)
			);
		}
		
		[Test]
		public void Push_Value()
		{
			Console.WriteLine(
				TestAssemblyAccessor.ParseMethod(
				MethodDef.Of(TestAssemblyAccessor.Assembly.MainModule, 
					delegate {
						object x = 10;
						object y = x;
					})
				)
			);
		}
		
		[Test]
		public void Push_Null()
		{
			Console.WriteLine(
				TestAssemblyAccessor.ParseMethod(
				MethodDef.Of(TestAssemblyAccessor.Assembly.MainModule, 
					delegate {
						Console.WriteLine("Pushing null: {0}", null);
					})
				)
			);
		}
				
		[Test]
		public void Add_Values_Int_Int()
		{
			Console.WriteLine(
				TestAssemblyAccessor.ParseMethod(
				MethodDef.Of(TestAssemblyAccessor.Assembly.MainModule, 
					delegate {
						int x = 10;
						Console.WriteLine(x + 10);
					})
				)
			);
		}
		
		[Test]
		public void Add_Values_Int_Single()
		{
			Console.WriteLine(
				TestAssemblyAccessor.ParseMethod(
				MethodDef.Of(TestAssemblyAccessor.Assembly.MainModule, 
					delegate {
						int x = 10;
						Console.WriteLine(x + 10f);
					})
				)
			);
		}

		[Test]
		public void Add_Values_Int_Double()
		{
			Console.WriteLine(
				TestAssemblyAccessor.ParseMethod(
				MethodDef.Of(TestAssemblyAccessor.Assembly.MainModule, 
					delegate {
						int x = 10;
						Console.WriteLine(x + 10.0);
					})
				)
			);
		}
		#endregion

		#region sizeof

		[Test]
		public unsafe void Sizeof()
		{
			Console.WriteLine(
				TestAssemblyAccessor.ParseMethod(
				MethodDef.Of(TestAssemblyAccessor.Assembly.MainModule, 
					delegate {
						unsafe {
							Console.WriteLine(sizeof(DateTime));
						}
					})
				)
			);
		}

		#endregion

		#region Metadata tokens

		[Test]
		public void Load_Method_Metadata_Token()
		{
			Console.WriteLine(
				TestAssemblyAccessor.ParseMethod(
				MethodDef.Of(TestAssemblyAccessor.Assembly.MainModule, 
					delegate {
						Console.WriteLine("Hello, {0}", new Func<ConsoleKeyInfo>(Console.ReadKey));
					})
				)
			);
		}

		#endregion
	}
}

