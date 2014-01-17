using System;
using Mono.Cecil.CodeDom.Rocks;
using NUnit.Framework;
using System.Runtime.ConstrainedExecution;

namespace Mono.Cecil.CodeDom.Tests
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
	}
}

