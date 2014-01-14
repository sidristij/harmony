using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace Mono.Cecil.CodeDom
{
	internal static class NodeEx
	{
		private static void Visit<T>([NotNull] this CodeDomExpression node, [CanBeNull] Action<T> preorderAction, [CanBeNull] Action<T> postorderAction) where T : CodeDomExpression
		{
			if (node == null)
				throw new ArgumentNullException("node");

			T obj = node as T;

			if (obj != null && preorderAction != null)
				preorderAction(obj);

			foreach (var subnode in node.Nodes)
				subnode.Visit(preorderAction, postorderAction);

			if (obj != null && postorderAction != null)
				postorderAction(obj);
		}

		public static void VisitPreorder<T>([NotNull] this CodeDomExpression node, [NotNull] Action<T> action) where T : CodeDomExpression
		{
			node.Visit(action, null);
		}

		public static void VisitPostorder<T>([NotNull] this CodeDomExpression node, [NotNull] Action<T> action) where T : CodeDomExpression
		{
			node.Visit(null, action);
		}

		public static T FindFirstPostorder<T>([CanBeNull] this CodeDomExpression node, [NotNull] Predicate<T> predicate) where T : CodeDomExpression
		{
			if (predicate == null)
				throw new ArgumentNullException("predicate");

			if (node == null)
				return default (T);

			foreach (CodeDomExpression subnode in node.Nodes)
			{
				var firstPostorder = subnode.FindFirstPostorder(predicate);
				if (firstPostorder != null)
					return firstPostorder;
			}

			T obj = node as T;
			return (obj == null || !predicate(obj)) ? default(T) : obj;
		}

		public static T FindNearestAncestor<T>([NotNull] this CodeDomExpression searchFrom, [CanBeNull] CodeDomExpression searchUntil = null, [CanBeNull] Predicate<T> predicate = null) where T : CodeDomExpression
		{
			if (searchFrom == null)
				throw new ArgumentNullException("searchFrom");

			for (var node = searchFrom; node != null; node = node.ParentNode)
			{
				var obj = node as T;

				if ((object) obj != null && (predicate == null || predicate(obj)))
					return obj;

				if (node == searchUntil)
					break;
			}

			return default (T);
		}

		public static CodeDomExpression GetRightSibling([NotNull] this CodeDomExpression node)
		{
			if (node == null)
				throw new ArgumentNullException("node");

			return (node.ParentNode != null) ? node.ParentNode.GetRightSibling() : null;
		}

		public static CodeDomExpression GetLeftSibling([NotNull] this CodeDomExpression node)
		{
			if (node == null)
				throw new ArgumentNullException("node");
			if (node.ParentNode != null)
				return node.ParentNode.GetLeftSibling();
			else
				return null;
		}

		public static CodeDomExpression GetFirstPostorder([NotNull] this CodeDomExpression node)
		{
			if (node == null)
				throw new ArgumentNullException("node");

			while (true)
			{
				var first = node.Nodes.FirstOrDefault();
				if (first != null)
					node = first;
				else
					break;
			}

			return node;
		}

		public static CodeDomExpression GetNextPostorder([NotNull] this CodeDomExpression node)
		{
			if (node == null)
				throw new ArgumentNullException("node");

			var rightSubling = node.GetRightSibling();
			if (rightSubling == null)
				return node.ParentNode;

			while (true)
			{
				var firstNode = rightSubling.Nodes.FirstOrDefault();
				if (firstNode != null)
					rightSubling = firstNode;
				else
					break;
			}
			return rightSubling;
		}

		public static T Detach<T>([CanBeNull] this T node) where T : CodeDomExpression
		{
			if (node == null)
				return default (T);

			if (!(node.ParentNode is CodeDomGroupExpression))
			{
				throw new ArgumentException("this.ParentNode should be group to be able to detach childs");
			}

			if (node.ParentNode != null)
			{
				var group = (node.ParentNode as CodeDomGroupExpression);
				group.RemoveAt(group.IndexOf(node));
			}

			return node;
		}

		public static IEnumerable<T> Detach<T>([NotNull] this IEnumerable<T> list) where T : CodeDomExpression
		{
			if (list == null)
				throw new ArgumentNullException("list");
			T[] objArray = list.ToArray();
			foreach (T node in objArray)
				node.Detach();
			return objArray;
		}
		/*
		public static bool DeepEquals(this CodeDomExpression thisNode, CodeDomExpression otherNode)
		{
			if (thisNode == null && otherNode == null)
				return true;
			if (thisNode == null || otherNode == null)
				return false;
			else
				return thisNode.DeepEqualityComparer.Equals(thisNode, otherNode);
		}

		public static bool DeepEquals(this INode[] theseNodes, INode[] otherNodes)
		{
			if (theseNodes == null && otherNodes == null)
				return true;
			if (theseNodes == null || otherNodes == null || theseNodes.Length != otherNodes.Length)
				return false;
			for (int index = 0; index < theseNodes.Length; ++index)
			{
				if (!NodeEx.DeepEquals(theseNodes[index], otherNodes[index]))
					return false;
			}
			return true;
		}
		*/
		/*
		public static void ValidateTree(this CodeDomExpression node)
		{
			NodeEx.ValidateTree(node, null);
		}

		private static void ValidateTree(CodeDomExpression node, CodeDomExpression parent)
		{
			foreach (var child in node.Nodes)
				NodeEx.ValidateTree(child, node);
		}
		/*
		public static T TypedClone<T>(this T node) where T : class, CodeDomExpression
		{
			if ((object) node != null)
				return (T) node.Clone();
			else
				return node;
		}

		public static T[] TypedClone<T>(this T[] nodes) where T : class, INode
		{
			if (nodes == null)
				return (T[]) null;
			T[] objArray = new T[nodes.Length];
			for (int index = 0; index < nodes.Length; ++index)
				objArray[index] = NodeEx.TypedClone<T>(nodes[index]);
			return objArray;
		}

		public static string ToStringDebug(this INode node)
		{
			if (node == null)
				return "<null>";
			StringWriter stringWriter = new StringWriter();
			CodeTextWriterAdapter codeWriter = new CodeTextWriterAdapter((TextWriter) stringWriter);
			TypeSwitchEx.TypeSwitch<INode>(node).Case<IDecompiledMethod>((Action<IDecompiledMethod>) (x => AstRenderer.RenderMethod((ICodeTextWriter) codeWriter, x))).Case<IStatement>((Action<IStatement>) (x => AstRenderer.RenderStatement((ICodeTextWriter) codeWriter, x, false))).Case<IExpression>((Action<IExpression>) (x => JetBrains.Decompiler.Render.CodeTextWriterEx.AppendNewLine(AstRenderer.RenderExpression((ICodeTextWriter) codeWriter, x)))).Case<ControlFlowBlockNode>((Action<ControlFlowBlockNode>) (x =>
			                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  {
				AstRenderer.RenderStatement((ICodeTextWriter) codeWriter, (IStatement) x.BlockStatement, false);
				if (x.BranchExpression == null)
					return;
				JetBrains.Decompiler.Render.CodeTextWriterEx.AppendNewLine(AstRenderer.RenderExpression(JetBrains.Decompiler.Render.CodeTextWriterEx.AppendText((ICodeTextWriter) codeWriter, "Branch condition: "), x.BranchExpression));
			})).EnsureHandled();
			return ((object) stringWriter.GetStringBuilder()).ToString().TrimEnd('\n', '\r');
		}
		*/
		public static HashSet<CodeDomExpression> GetSubTreeNodeSet([NotNull] this CodeDomExpression node)
		{
			if (node == null)
				throw new ArgumentNullException("node");

			HashSet<CodeDomExpression> subTree = new HashSet<CodeDomExpression>();
			node.VisitPostorder<CodeDomExpression>(subNode => subTree.Add(subNode));
			return subTree;
		}
	}
}
