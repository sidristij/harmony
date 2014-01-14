using System;
using Mono.Cecil;
using Mono.Cecil.Rocks;
using Mono.Cecil.CodeDom.Rocks;
using Mono.Cecil.CodeDom.Parser;
using System.Collections.Generic;
using Mono.Cecil.CodeDom;
using System.Linq;
using Mono.Cecil.CodeDom.Parser.Tcf;

namespace Harmony.Test
{
	static class UnparsedWalker
	{
		public static IEnumerable<TResult> SelectNodes<TResult>(this CodeDomExpression node, Predicate<TResult> checker = null) where TResult: class 
		{
			var queue = new Queue<CodeDomExpression>();
			queue.Enqueue(node);
			while(queue.Count > 0)
			{
				var current = queue.Dequeue();
				var ok = checker == null || checker(current as TResult);
				if ((current is TResult) && ok)
				{
					yield return current as TResult;
				}

				foreach (var item in current.Nodes)
				{
					queue.Enqueue(item);
				}
			}
		}
	}

	class MainClass
	{
		public static void Main(string[] args)
		{
			var assembly = AssemblyDefinition.ReadAssembly("Mono.Cecil.CodeDom.dll");
			var methoddef = MethodDef.Of<int, object>(assembly.MainModule, TestMethod);

			methoddef.Body.SimplifyMacros();

			var parser = new CodeDomBranchesParser();
			var codeparser = new CodeDomParser();
			var context = new Context(methoddef , parser);

			var superNode = parser.ParseMethodRoot(context);
			parser.ParseTcfBlocks(context);
			parser.ParseBranches(context, superNode.SelectNodes<CodeDomUnparsedExpression>().ToList());

			foreach(var unparsed in superNode.SelectNodes<CodeDomUnparsedExpression>())
			{
				var parsed = codeparser.Parse(methoddef, unparsed.Instructions.First, unparsed.Instructions.Last, unparsed.ParentNode as CatchBlockExpression);
				unparsed.ReplaceWith(parsed);
			}


			Console.WriteLine(superNode);
			Console.ReadKey();
		}

		private static void TestMethod(int integer, object simpleObject)
		{
			try {
				if (simpleObject != null)
				{
					Console.WriteLine("Hello, {0} and {1}", integer-10+5*integer/75, simpleObject);
				}
			}
			finally
			{
				while (integer - 6 > 0)
				{
					Console.WriteLine("in loop");
				}
			}
		}
	}
}
