using System;
using NUnit.Framework;
using Mono.Cecil.CodeDom.Rocks;

namespace Mono.Cecil.CodeDom.Tests
{
	[TestFixture]
	public class TcfIfElse
	{
		[Test]
		public void If_Inside_TryCatch_InTry()
		{
			Console.WriteLine(
				TestAssemblyAccessor.ParseMethod(
				MethodDef.Of(TestAssemblyAccessor.Assembly.MainModule,
					delegate
					{
						try
						{
							int i = 0;
							if(i == 0)
							{
								Console.WriteLine("True block");
							}
						}
						finally
						{
							Console.WriteLine("Anyway");
						}
					})
				)
			);
		}

		[Test]
		public void IfElse_Inside_TryCatch_InTry()
		{
			Console.WriteLine(
				TestAssemblyAccessor.ParseMethod(
				MethodDef.Of(TestAssemblyAccessor.Assembly.MainModule,
					delegate
					{
						try
						{
							int i = 0;
							if(i == 0)
							{
								Console.WriteLine("True block");
							}
							else 
							{
								Console.WriteLine("False block");
							}
						}
						finally
						{
							Console.WriteLine("Anyway");
						}
					})
				)
			);
		}

		[Test]
		public void If_Inside_TryCatch_InCatch()
		{
			Console.WriteLine(
				TestAssemblyAccessor.ParseMethod(
				MethodDef.Of(TestAssemblyAccessor.Assembly.MainModule,
					delegate
					{
						int i = 0;
						try
						{
							Console.WriteLine("Try block");
						}
						catch(ArgumentException ex)
						{
							if(i == 0)
							{
								Console.WriteLine("True block: {0}", ex);
							}
						}
						finally
						{
							Console.WriteLine("Anyway");
						}
					})
				)
			);
		}

		[Test]
		public void IfElse_Inside_TryCatch_InCatch()
		{
			Console.WriteLine(
				TestAssemblyAccessor.ParseMethod(
				MethodDef.Of(TestAssemblyAccessor.Assembly.MainModule,
					delegate
					{
						int i = 0;
						try
						{
							Console.WriteLine("Try block");
						}
						catch(ArgumentException ex)
						{
							if(i == 0)
							{
								Console.WriteLine("True block: {0}", ex);
							}
							else 
							{
								Console.WriteLine("False block: {0}", ex);
							}
						}
						finally
						{
							Console.WriteLine("Anyway");
						}
					})
				)
			);
		}

		[Test]
		public void If_Inside_TryCatch_InFinally()
		{
			Console.WriteLine(
				TestAssemblyAccessor.ParseMethod(
				MethodDef.Of(TestAssemblyAccessor.Assembly.MainModule,
					delegate
					{
						int i = 0;
						try
						{
							Console.WriteLine("Try block");
						}
						catch(ArgumentException ex)
						{
							Console.WriteLine("Catch block: {0}", ex);
						}
						finally
						{
							if(i == 0)
							{
								Console.WriteLine("True block");
							}
						}
					})
				)
			);
		}

		
		[Test]
		public void IfElse_Inside_TryCatch_InFinally()
		{
			Console.WriteLine(
				TestAssemblyAccessor.ParseMethod(
				MethodDef.Of(TestAssemblyAccessor.Assembly.MainModule,
					delegate
					{
						int i = 0;
						try
						{
							Console.WriteLine("Try block");
						}
						catch(ArgumentException ex)
						{
							Console.WriteLine("Catch block: {0}", ex);
						}
						finally
						{
							if(i == 0)
							{
								Console.WriteLine("True block");
							}
							else 
							{
								Console.WriteLine("False block");
							}
						}
					})
				)
			);
		}
	}
}

