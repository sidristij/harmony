using System.Linq;

using Mono.Cecil.CodeDom.Parser;
using Mono.Cecil.CodeDom.Parser.Tcf;
using Mono.Cecil.Rocks;

namespace Mono.Cecil.CodeDom.Tests
{
	public class TestAssemblyAccessor
	{
		static TestAssemblyAccessor accessor;
		static AssemblyDefinition assembly;

		public static AssemblyDefinition Assembly 
		{ 
			get { 
				return assembly ?? (assembly = AssemblyDefinition.ReadAssembly("Mono.Cecil.CodeDom.Tests.dll")); 
			}
		}

		public static CodeDomExpression ParseMethod(MethodDefinition method_def)
		{
			method_def.Body.SimplifyMacros();

			var parser = new CodeDomBranchesParser();
			var codeparser = new CodeDomParser();
			var context = new Context(method_def , parser);

			var superNode = parser.ParseMethodRoot(context);
			parser.ParseTcfBlocks(context);
			parser.ParseBranches(context, NodeEx.SelectNodes<CodeDomUnparsedExpression>(superNode).ToList());

			foreach(var unparsed in superNode.SelectNodes<CodeDomUnparsedExpression>())
			{
				var parsed = codeparser.Parse(method_def, unparsed.Instructions.First, unparsed.Instructions.Last, unparsed.ParentNode as CatchBlockExpression);
				unparsed.ReplaceWith(parsed);
			}

			return superNode;
		}
	}
}

