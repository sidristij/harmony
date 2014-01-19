using System;
using Mono.Cecil.Cil;
using Mono.Cecil.CodeDom.Collections;
using System.Linq;
using System.Collections.Generic;
using Mono.Cecil.CodeDom.Parser.Tcf;

namespace Mono.Cecil.CodeDom
{
	/// <summary>
	/// Base class for all CodeDOM nodes. Can be used for untyped nodes like nodes groups
	/// </summary>
	public class CodeDomExpression : IEnumerable<CodeDomExpression>
	{
		private readonly Context _context;

		static CodeDomExpression()
		{
		}

		public CodeDomExpression(Context context)
		{
			_context = context;
			Nodes = FixedList<CodeDomExpression>.Empty;
			ReturnType = Context.MakeRef(typeof(void));
		}

		public CodeDomExpression(Context context, CodeDomExpression parent) : this(context)
		{
			ParentNode = parent;
		}

		public virtual void ResetInstructionsInMap()
		{
			;
		}

		public void ReplaceWith(CodeDomExpression another)
		{
			var ind = ParentNode.Nodes.IndexOf(this);
			ParentNode.Nodes[ind] = another;

		}

		/// <summary>
		/// Gets ending string to be inserted after isFinally node.ToString() expression.
		/// </summary>
		public string FinalString
		{
			get {
				return IsFinal ? string.Format(";{0}", Environment.NewLine) : "";
			}
		}

		/// <summary>
		/// Gets parent node in CodeDom tree
		/// </summary>
		public CodeDomExpression ParentNode { get; internal set; }

		/// <summary>
		/// Gets CodeDOM Root node.
		/// </summary>
		public CodeDomExpression RootNode {  get {  return IsEmpty ? this : ParentNode.RootNode; } }

		/// <summary>
		/// Gets related method body instance reference 
		/// </summary>
		/// <value>The method body.</value>
		public MethodBody MethodBody 
		{ 
			get { 
				return Context.Method.Body; 
			} 
		}

		/// <summary>
		/// Enumerates all catch blocks, which covers current
		/// </summary>
		/// <returns>The catch blocks.</returns>
		public IEnumerable<CatchBlockExpression> GetCatchBlocks()
		{
			var node = ParentNode;
			var root = RootNode;
			do {
				var catchBlock = node as CatchBlockExpression;
				if(catchBlock != null)
				{
					yield return catchBlock;
				}
				node = node.ParentNode;
			} while(node != root);
		}

		/// <summary>
		/// Gets parsing context.
		/// </summary>
		public Context Context { get { return _context; } }

		/// <summary>
		/// Gets return value by this node. This means, as result of interpretiong all instructions in this node will be object of ReturnType type in the stack.
		/// </summary>
		public TypeReference ReturnType { get; protected set; }

		/// <summary>
		/// How many items this node will read from stack (0 .. ?).
		/// </summary>
		public int ReadsStack { get; protected set; }

		/// <summary>
		/// How many slots will be pushed to stack as result (0 or 1).
		/// </summary>
		/// <value>The writes stack.</value>
		public int WritesStack { get; protected set; }

		/// <summary>
		/// Indicates whether node has no resulting slots in stack and contains solid expession.
		/// </summary>
		public virtual bool IsFinal { get { return WritesStack == 0; } } 

		/// <summary>
		/// Indicates whether node is branches execution flow.
		/// </summary>
		public virtual bool IsBranches { get { return false; } }

		/// <summary>
		/// Indicates whether node is parsed and contains valid nodes
		/// </summary>
		/// <value><c>true</c> if this instance is parsed; otherwise, <c>false</c>.</value>
		public virtual bool IsParsed 
		{ 
			get 
			{ 
				return Nodes.All(node => node.IsParsed); 
			} 
		}

		/// <summary>
		/// Indicates whether node contains subnodes
		/// </summary>
		/// <value><c>true</c> if this instance is group; otherwise, <c>false</c>.</value>
		public virtual bool IsGroup { get { return false; } }

		/// <summary>
		/// Indicates whether node contains subnodes and user side can add or remove subnodes
		/// </summary>
		/// <value><c>true</c> if this instance is group; otherwise, <c>false</c>.</value>
		public virtual bool IsExtandableGroup { get { return false; } }

		/// <summary>
		/// Gets all dependent subnodes. These nodes could be methods parameters, simple expressions parameters, or, maybe branches.
		/// </summary>
		public virtual IFixedList<CodeDomExpression> Nodes { get; protected set; }

		/// <summary>
		/// Indicates that node contains nothing. Like nop
		/// </summary>
		/// <value><c>true</c> if this instance is empty; otherwise, <c>false</c>.</value>
		public virtual bool IsEmpty { get { return true; } }

		public string Hash { get { return GetHashCode().ToString(); } }

		#region IEnumerable implementation

		public IEnumerator<CodeDomExpression> GetEnumerator()
		{
			return Nodes.GetEnumerator();
		}

		#endregion

		#region IEnumerable implementation

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion

		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns>
		/// A string that represents the current object.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override string ToString()
		{
			return string.Join(Environment.NewLine, Nodes);
		}
	}
}

