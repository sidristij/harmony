using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil.Cil;
using Mono.Cecil.CodeDom.Collections;

namespace Mono.Cecil.CodeDom.Parser.Tcf
{
	public sealed class TryExpression : CodeDomExpression
	{
		public const int TryPos = 0;
		public const int FaultPos = 1;
		public const int FilterPos = 2;
		public const int FinallyPos = 3;
		public const int CatchPos = 4;
		private const int MaxNodes = CatchPos;

		private readonly MergedList<CodeDomExpression> _mergedList = new MergedList<CodeDomExpression>();
		private readonly IFixedList<CodeDomExpression> _handlersContainer = new FixedList<CodeDomExpression>(MaxNodes);

		public TryExpression(Context context, List<ExceptionHandler> linkedHandlers,
		                     CodeDomExpression exp_try, CodeDomExpression exp_fault, CodeDomExpression exp_filter, CodeDomExpression exp_finally, params CatchBlockExpression[] exp_catches)
			: base(context)
		{
			// Setup merged readonly access
			Nodes = _mergedList;

			// Handlers blocks
			Body = exp_try;
			Fault = exp_fault;
			Filter = exp_filter;
			Finally = exp_finally;
			Handlers = exp_catches.ToList();

			ExceptionHandlers = new FixedListAdapter<ExceptionHandler>(linkedHandlers);

			// make adapter for fixed-size access 
			var listToFixAdapter = new FixedListByDelegate<CodeDomExpression>(
				i => Handlers[i],
				delegate(int i, CodeDomExpression val) { Handlers[i] = (CatchBlockExpression)val; },
			() => Handlers.Count);

			// finish merged access setup
			_mergedList.AddList(_handlersContainer);
			_mergedList.AddList(listToFixAdapter);

			InstructionStart = linkedHandlers.First().TryStart;
			InstructionEnd = linkedHandlers.Last().HandlerEnd;

			// setup parents
			foreach (var node in Nodes)
			{
				node.ParentNode = this;
			}
		}

		public IFixedList<ExceptionHandler> ExceptionHandlers
		{
			get; private set;
		}

		public Instruction InstructionStart
		{
			get; private set;
		}

		public Instruction InstructionEnd
		{
			get; private set;
		}

		/// <summary>
		/// Gets "try" block
		/// </summary>
		public CodeDomExpression Body
		{
			get { return _handlersContainer[TryPos]; } 
			private set { _handlersContainer[TryPos] = value; value.ParentNode = this; }
		}

		/// <summary>
		/// Gets "fault" block
		/// </summary>
		public CodeDomExpression Fault
		{
			get { return _handlersContainer[FaultPos]; }
			private set { _handlersContainer[FaultPos] = value; value.ParentNode = this; }
		}

		/// <summary>
		/// Gets "filter" block
		/// </summary>
		public CodeDomExpression Filter
		{
			get { return _handlersContainer[FilterPos]; }
			private set { _handlersContainer[FilterPos] = value; value.ParentNode = this; }
		}

		/// <summary>
		/// Gets "finally" block
		/// </summary>
		public CodeDomExpression Finally
		{
			get { return _handlersContainer[FinallyPos]; }
			private set { _handlersContainer[FinallyPos] = value; value.ParentNode = this; }
		}

		/// <summary>
		/// Gets list of "catch" blocks
		/// </summary>
		/// <value>The handlers.</value>
		public List<CatchBlockExpression> Handlers { get; private set; }

		public override string ToString()
		{
			return string.Format("try {{ {0} }} {1} {2}",
			                     Body,
			                     string.Join(Environment.NewLine, Handlers),
			                     Finally.IsEmpty ? "" : string.Format("finally {{ {0} }}", Finally));
		}
	}
}

