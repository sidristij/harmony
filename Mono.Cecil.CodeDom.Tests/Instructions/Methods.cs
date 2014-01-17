using System;
using NUnit.Framework;
using Mono.Cecil.CodeDom.Rocks;
using System.Collections.Generic;

namespace Mono.Cecil.CodeDom.Tests.Instructions
{
	[TestFixture]
	public class Methods
	{
		#region Static methods

		public void Call_Void_No_Parameters_Method_Method()
		{
			Console.WriteLine();
		}

		[Test]
		public void Call_Void_No_Parameters_Method()
		{
			Console.WriteLine(TestAssemblyAccessor.ParseMethod(MethodDef.Of(TestAssemblyAccessor.Assembly.MainModule, Call_Void_No_Parameters_Method_Method)));
		}

		public void Call_Void_With_Inline_Parameters_Method_Method()
		{
			Console.WriteLine("string");
		}

		[Test]
		public void Call_Void_With_Inline_Parameters_Method()
		{
			Console.WriteLine(TestAssemblyAccessor.ParseMethod(MethodDef.Of(TestAssemblyAccessor.Assembly.MainModule, Call_Void_With_Inline_Parameters_Method_Method)));
		}

		public void Call_Void_With_Parameters_Method_Method()
		{
			int x = 5;
			string format = "String: {0}";
			Console.WriteLine(format, x.ToString());
		}

		[Test]
		public void Call_Void_With_Parameters_Method()
		{
			Console.WriteLine(TestAssemblyAccessor.ParseMethod(MethodDef.Of(TestAssemblyAccessor.Assembly.MainModule, Call_Void_With_Parameters_Method_Method)));
		}

		#endregion

		#region Instance methods

		private void TestMethod()
		{
		}

		public void Call_Static_Void_No_Parameters_Method_Method()
		{
			TestMethod();
		}

		[Test]
		public void Call_Static_Void_No_Parameters_Method()
		{
			Console.WriteLine(TestAssemblyAccessor.ParseMethod(MethodDef.Of(TestAssemblyAccessor.Assembly.MainModule, Call_Static_Void_No_Parameters_Method_Method)));
		}

		private void TestMethod2(string param1)
		{
			Console.WriteLine(param1);
		}

		public void Call_Static_Void_With_Inline_Parameters_Method_Method()
		{
			TestMethod2("string");
		}

		[Test]
		public void Call_Static_Void_With_Inline_Parameters_Method()
		{
			Console.WriteLine(TestAssemblyAccessor.ParseMethod(MethodDef.Of(TestAssemblyAccessor.Assembly.MainModule, Call_Static_Void_With_Inline_Parameters_Method_Method)));
		}

		private void TestMethod3(string param1, int param2)
		{
			Console.WriteLine(param1, param2);
		}

		public void Call_Static_Void_With_Parameters_Method_Method()
		{
			int x = 5;
			string format = "String: {0}";
			TestMethod3(format, x);
		}

		[Test]
		public void Call_Static_Void_With_Parameters_Method()
		{
			Console.WriteLine(TestAssemblyAccessor.ParseMethod(MethodDef.Of(TestAssemblyAccessor.Assembly.MainModule, Call_Static_Void_With_Parameters_Method_Method)));
		}

		#endregion

		#region Creating objects

		public void Call_Create_No_Parameters_Method()
		{
			var obj = new object();
		}

		[Test]
		public void Call_Create_No_Parameters()
		{
			Console.WriteLine(TestAssemblyAccessor.ParseMethod(MethodDef.Of(TestAssemblyAccessor.Assembly.MainModule, Call_Create_No_Parameters_Method)));
		}
		
		public void Call_Create_With_Parameters_Method()
		{
			var obj = new List<int>(10);
		}

		[Test]
		public void Call_Create_With_Parameters()
		{
			Console.WriteLine(TestAssemblyAccessor.ParseMethod(MethodDef.Of(TestAssemblyAccessor.Assembly.MainModule, Call_Create_With_Parameters_Method)));
		}

		#endregion

		#region Creating structs
		
		public void Call_InitStruct_No_Params_Method()
		{
			Console.WriteLine(new DateTime());
		}

		[Test]
		public void Call_InitStruct_No_Params()
		{
			Console.WriteLine(TestAssemblyAccessor.ParseMethod(MethodDef.Of(TestAssemblyAccessor.Assembly.MainModule, Call_InitStruct_No_Params_Method)));
		}
		
		public void Call_InitStruct_With_Params_Method()
		{
			Console.WriteLine(new DateTime(2013, 10, 30));
		}

		[Test]
		public void Call_InitStruct_With_Params()
		{
			Console.WriteLine(TestAssemblyAccessor.ParseMethod(MethodDef.Of(TestAssemblyAccessor.Assembly.MainModule, Call_InitStruct_With_Params_Method)));
		}

		#endregion
	}
}

