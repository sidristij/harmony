using System;
using Mono.Cecil.CodeDom.Rocks;
using NUnit.Framework;

namespace Mono.Cecil.CodeDom.Tests.Branching.IfElse
{
    [TestFixture]
    class SimpleConditions
    {
        [Test]
        public void NoBefore_NoAfter_Just_If_Boolean_Condition()
        {
            Console.WriteLine(
                TestAssemblyAccessor.ParseMethod(
                MethodDef.Of(TestAssemblyAccessor.Assembly.MainModule,
                    delegate
                    {
                        if(Console.ReadKey().GetHashCode() < 10)
                        {
                            Console.WriteLine("Cool");
                        }
                    })
                )
            );
        }

        [Test]
        public void NoBefore_NoAfter_If_Else()
        {
            Console.WriteLine(
                TestAssemblyAccessor.ParseMethod(
                MethodDef.Of(TestAssemblyAccessor.Assembly.MainModule,
                    delegate
                    {
                        if (Console.ReadKey().GetHashCode() < 10)
                        {
                            Console.WriteLine("Cool");
                        }
                        else
                        {
                            Console.WriteLine("Illegal arguments");
                        }
                    })
                )
            );
        }

        [Test]
        public void Before_NoAfter_If()
        {
            Console.WriteLine(
                TestAssemblyAccessor.ParseMethod(
                MethodDef.Of(TestAssemblyAccessor.Assembly.MainModule,
                    delegate
                    {
                        var x = Console.ReadKey().GetHashCode();
                        if (x < 10)
                        {
                            Console.WriteLine("x value is: {0}", x);
                        }
                    })
                )
            );
        }

        [Test]
        public void Before_NoAfter_If_Else()
        {
            Console.WriteLine(
                TestAssemblyAccessor.ParseMethod(
                MethodDef.Of(TestAssemblyAccessor.Assembly.MainModule,
                    delegate
                    {
                        var x = Console.ReadKey().GetHashCode();
                        if (x < 10)
                        {
                            Console.WriteLine("x value is: {0}", x);
                        } else
                        {
                            Console.WriteLine("different x value is: {0}", -x);
                        }
                    })
                )
            );
        }

        [Test]
        public void Before_After_If()
        {
            Console.WriteLine(
                TestAssemblyAccessor.ParseMethod(
                MethodDef.Of(TestAssemblyAccessor.Assembly.MainModule,
                    delegate
                    {
                        var x = Console.ReadKey().GetHashCode();
                        if (x < 10)
                        {
                            Console.WriteLine("x value is: {0}", x);
                        }
                        Console.WriteLine("all is ok");
                    })
                )
            );
        }

        [Test]
        public void Before_After_If_Else()
        {
            Console.WriteLine(
                TestAssemblyAccessor.ParseMethod(
                MethodDef.Of(TestAssemblyAccessor.Assembly.MainModule,
                    delegate
                    {
                        var x = Console.ReadKey().GetHashCode();
                        if (x < 10)
                        {
                            Console.WriteLine("x value is: {0}", x);
                        }
                        else
                        {
                            Console.WriteLine("different x value is: {0}", -x);
                        }
                        Console.WriteLine("all is ok");
                    })
                )
            );
        }
    }
}
