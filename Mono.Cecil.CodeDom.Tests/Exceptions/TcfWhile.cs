using System;
using NUnit.Framework;
using Mono.Cecil.CodeDom.Rocks;

namespace Mono.Cecil.CodeDom.Tests
{
	[TestFixture]
	public class TcfWhile
	{
		[Test]
		public void While_Inside_TryCatch_InTry()
		{
			Console.WriteLine(
				TestAssemblyAccessor.ParseMethod(
				MethodDef.Of(TestAssemblyAccessor.Assembly.MainModule,
					delegate
					{
						try
						{
							int i = 0;
							while(i == 0)
							{
								Console.WriteLine("True block");
								i++;
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
	}
}

