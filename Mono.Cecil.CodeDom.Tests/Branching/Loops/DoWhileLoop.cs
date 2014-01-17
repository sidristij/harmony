using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil.CodeDom.Rocks;
using NUnit.Framework;

namespace Mono.Cecil.CodeDom.Tests.Branching.Loops
{
    [TestFixture]
    public class DoWhileLoop
    {
        [Test]
        public void Simple_Condition()
        {
            Console.WriteLine(
                TestAssemblyAccessor.ParseMethod(
                MethodDef.Of(TestAssemblyAccessor.Assembly.MainModule,
                    delegate
                        {
                            do
                            {
                                Console.WriteLine("Cool");
                            } while (Console.ReadKey().GetHashCode() < 10);
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

                        do
                        {
                            Console.WriteLine("Cool");
                        } while (Console.ReadKey().GetHashCode() < 10);
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

                        do
                        {
                            Console.WriteLine("Cool");
                        } while (Console.ReadKey().GetHashCode() < 10);

                        Console.ReadKey();
                    })
                )
            );
        }

    }
}
