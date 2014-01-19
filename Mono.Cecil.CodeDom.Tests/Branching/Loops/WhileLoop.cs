using System;
using Mono.Cecil.CodeDom.Rocks;
using NUnit.Framework;

namespace Mono.Cecil.CodeDom.Tests.Branching.Loops
{
    [TestFixture]
    public class WhileLoop
    {
        [Test]
        public void Simple_Condition()
        {
            Console.WriteLine(
                TestAssemblyAccessor.ParseMethod(
                MethodDef.Of(TestAssemblyAccessor.Assembly.MainModule,
                    delegate
                    {
                        while(Console.ReadKey().GetHashCode() < 10)
                        {
                            Console.WriteLine("Cool");
                        }
                    })
                )
            );
        }

        [Test]
        public void Simple_Condition_Before()
        {
            Console.WriteLine(
                TestAssemblyAccessor.ParseMethod(
                MethodDef.Of(TestAssemblyAccessor.Assembly.MainModule,
                    delegate
                    {
                        Console.WriteLine("hello, potatoes!");

                        while (Console.ReadKey().GetHashCode() < 10)
                        {
                            Console.WriteLine("Cool");
                        }
                    })
                )
            );
        }

        [Test]
        public void Simple_Condition_Before_After()
        {
            Console.WriteLine(
                TestAssemblyAccessor.ParseMethod(
                MethodDef.Of(TestAssemblyAccessor.Assembly.MainModule,
                    delegate
                    {
                        Console.WriteLine("hello, potatoes!");

                        while (Console.ReadKey().GetHashCode() < 10)
                        {
                            Console.WriteLine("Cool");
                        }

                        Console.ReadKey();
                    })
                )
            );
        }

    }
}
