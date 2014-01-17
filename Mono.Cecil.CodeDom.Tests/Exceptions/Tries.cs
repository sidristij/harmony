using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil.CodeDom.Rocks;
using NUnit.Framework;

namespace Mono.Cecil.CodeDom.Tests.Exceptions
{
    [TestFixture]
    public class Tries
    {

        [Test]
        public void Try_Finally_Method()
        {
            Console.WriteLine(
                TestAssemblyAccessor.ParseMethod(
                MethodDef.Of(TestAssemblyAccessor.Assembly.MainModule,
                    delegate
                    {
                        try
                        {
                            Console.WriteLine("Dangerous");
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
        public void Try_Catch_Finally_Method()
        {
            Console.WriteLine(
                TestAssemblyAccessor.ParseMethod(
                MethodDef.Of(TestAssemblyAccessor.Assembly.MainModule,
                    delegate
                    {
                        try
                        {
                            Console.WriteLine("Dangerous");
                        }
                        catch
                        {
                           Console.WriteLine("Information about exception");     
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
        public void Try_Catches_Finally_Method()
        {
            Console.WriteLine(
                TestAssemblyAccessor.ParseMethod(
                MethodDef.Of(TestAssemblyAccessor.Assembly.MainModule,
                    delegate
                    {
                        try
                        {
                            Console.WriteLine("Dangerous");
                        }
                        catch (ArgumentOutOfRangeException exception1)
                        {
                            Console.WriteLine("Information about exception1");
                        }
                        catch (Exception exception2)
                        {
                            Console.WriteLine("Information about exception2");
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
        public void Try_Catch_NoFinally_Method()
        {
            Console.WriteLine(
                TestAssemblyAccessor.ParseMethod(
                MethodDef.Of(TestAssemblyAccessor.Assembly.MainModule,
                    delegate
                    {
                        try
                        {
                            Console.WriteLine("Dangerous");
                        }
                        catch (Exception exception1)
                        {
                            Console.WriteLine("Information about exception1");
                        }
                    })
                )
            );
        }
    }
}
