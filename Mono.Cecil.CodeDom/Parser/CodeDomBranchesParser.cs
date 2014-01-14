using System;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using System.Linq;
using Mono.Cecil.CodeDom.Parser.Branching;
using Mono.Cecil.CodeDom.Parser.Tcf;

namespace Mono.Cecil.CodeDom.Parser
{
	public class CodeDomBranchesParser : CodeDomParserBase
	{
		private readonly Dictionary<Instruction, Instruction> _incoming = new Dictionary<Instruction, Instruction>();
		private readonly List<Instruction> _processed = new List<Instruction>();

		#region Step 0: Method root

		/// <summary>
		/// Light method to help to make base nodes for method root.
		/// </summary>
		public CodeDomGroupExpression ParseMethodRoot(Context context)
		{
			var group = new CodeDomGroupExpression(context);
			var node = new CodeDomUnparsedExpression(context , context.Method.Body.Instructions.First() , context.Method.Body.Instructions.Last());
			group.Add(node);
			return group;
		}

		#endregion

		#region Step 1: Parsing all try-catch-finally blocks

		/// <summary>
		/// Parses all Tcf blocks. Should be called after ParseBranches. 
		/// </summary>
		/// <param name="context"></param>
		public void ParseTcfBlocks(Context context)
		{
			var cecilHandlers = context.Method.Body.ExceptionHandlers;

			// Groups of handlers - when one Try have many Catches or/and Finally block
			var groups = cecilHandlers.GroupBy(item => new { item.TryStart, item.TryEnd });

			foreach (var grouping in groups)
			{
				var tryinfo = grouping.First();

				// split if needed, and return Expression, that covers TCF block
				var tcfblock = SplitGroupAndReplace(context, tryinfo.TryStart, grouping.Last().HandlerEnd.Previous, groupit: true);
				var tryblock = SplitGroupAndReplace(context, tryinfo.TryStart, tryinfo.TryEnd.Previous, exp_root: tcfblock);

				CodeDomExpression faultBlock = null;
				CodeDomExpression finallyBlock = null;
				CodeDomExpression filterBlock = null;
				List<CatchBlockExpression> catches = new List<CatchBlockExpression>();

				foreach (var handler in grouping)
				{
					var group = SplitGroupAndReplace(context, handler.HandlerStart, handler.HandlerEnd.Previous, exp_root: tcfblock);

					switch (handler.HandlerType)
					{
						case ExceptionHandlerType.Catch:
							catches.Add(new CatchBlockExpression(context, handler.CatchType, group));
							break;

						case ExceptionHandlerType.Filter:
							filterBlock = group;
							break;

						case ExceptionHandlerType.Finally:
							finallyBlock = group;
							break;

						case ExceptionHandlerType.Fault:
							faultBlock = group;
							break;
					}
				}

				// create uncreated
				if (filterBlock == null) filterBlock = new CodeDomExpression(context);
				if (finallyBlock == null) finallyBlock = new CodeDomExpression(context);
				if (faultBlock == null) faultBlock = new CodeDomExpression(context);

				// replace with our result
				var tcf = new TryExpression(context, grouping.ToList(), tryblock, faultBlock, filterBlock, finallyBlock, catches.ToArray());
				tcfblock.ParentNode.Nodes[tcfblock.ParentNode.Nodes.IndexOf(tcfblock)] = tcf;
				tcf.ParentNode = tcfblock.ParentNode;
			}
		}

		/// <summary>
		/// Should be used to split abstract instructions range.
		/// Returns Expression, that covers given instructions range
		/// </summary>
		private CodeDomExpression SplitGroupAndReplace(Context context, Instruction start, Instruction end, bool groupit = false, CodeDomExpression exp_root = null)
		{
			var gr_left = context.GetExpression(start);
			var gr_right = context.GetExpression(end);
			var root = exp_root ?? GetRootForNodes(gr_left, gr_right);

			if (root == null)
			{
				throw new ArgumentException("Given instructions have different roots");
			}

			gr_left = LookupNearestSubnode(root, gr_left);
			gr_right = LookupNearestSubnode(root, gr_right);

			// if both sides in one node
			if (gr_left == gr_right)
			{
				// if this node isn't parsed 
				if (gr_left is CodeDomUnparsedExpression)
				{
					// replace node with group of three subnodes, where
					// one subnode contains our instructions range.
					var node = gr_left as CodeDomUnparsedExpression;
					CodeDomExpression coverto;
					var last = node.Instructions.Last;
					var group = node.ParentNode as CodeDomGroupExpression;
					var index = group.Nodes.IndexOf(node);

					if (start != node.Instructions.First)
					{
						node.Instructions.Last = start.Previous;
						index++;
					}
					else
						group.RemoveAt(index);

					var item = new CodeDomUnparsedExpression(context, start, end);
					if (groupit)
						group.Insert(index, coverto = new CodeDomGroupExpression(context) { item });
					else
						group.Insert(index, coverto = item);
					index++;

					if (end != node.Instructions.Last)
						group.Insert(index, new CodeDomUnparsedExpression(context, end.Next, last));

					return coverto;
				}

				// node is parsed (may be if-else): nothing to split
				return gr_left;
			}
			else
			{
				if (!(gr_left.ParentNode is CodeDomGroupExpression))
					throw new ArgumentException("nodes root should be group");

				var rootgroup = gr_left.ParentNode as CodeDomGroupExpression;
				var group = new CodeDomGroupExpression(context);

				var i_left = rootgroup.IndexOf(gr_left);
				var i_right = rootgroup.IndexOf(gr_right);

				// parse left side if unparsed or include it whole
				if (gr_left is CodeDomUnparsedExpression && (gr_left as CodeDomUnparsedExpression).Instructions.First != start)
				{
					var node = gr_left as CodeDomUnparsedExpression;
					node.Instructions.Last = start.Previous;
					group.Add(new CodeDomUnparsedExpression(context, start, node.Instructions.Last));
					node.ResetInstructionsInMap();
				}
				else
				{
					rootgroup.Remove(gr_left);
					group.Add(gr_left);
				}

				// move all between side expressions into group
				while (i_right != i_left + 1)
				{
					group.Add(rootgroup[i_left + 1]);
					rootgroup.RemoveAt(i_left + 1);
					i_right--;
				}

				if (gr_right is CodeDomUnparsedExpression && (gr_right as CodeDomUnparsedExpression).Instructions.Last != end)
				{
					var node = gr_right as CodeDomUnparsedExpression;
					group.Add(new CodeDomUnparsedExpression(context, node.Instructions.First, end));
					node.Instructions.First = end;
					node.ResetInstructionsInMap();
				}
				else
				{
					rootgroup.Remove(gr_right);
					group.Add(gr_right);
				}

				// insert all grouped to the root
				rootgroup.Insert(i_left, group);

				return group;
			}
		}

		/// <summary>
		/// Looks up by parents to root's child in the same subtree.
		/// </summary>
		private CodeDomExpression LookupNearestSubnode(CodeDomExpression root, CodeDomExpression node)
		{
			while (node.ParentNode != root)
				node = node.ParentNode;
			return node;
		}

		/// <summary>
		/// Looks for nearest to leafes parent node
		/// </summary>
		private CodeDomExpression GetRootForNodes(CodeDomExpression left, CodeDomExpression right)
		{
			var parents = new List<CodeDomExpression>();
			bool first = true;
			while (right != null)
			{
				if (!first) parents.Add(right);
				first = false;
				right = right.ParentNode;
			}

			first = true;
			while (left != null)
			{
				if (!first && parents.Contains(left))
				{
					return left;
				}
				first = false;
				left = left.ParentNode;
			}
			return null;
		}

		#endregion

		#region Step 2a: For if-else-while: calculate which instructions are the targets of jump instructions.

		public void ParseBranches(Context context, List<CodeDomUnparsedExpression> items)
		{
			CalculateIncomingJumps(context.Method.Body);
			foreach (var item in items)
			{
				ParseInstructionsBlock(context, item);
			}
		}

		#endregion

		#region Step 2b: For if-else-while: Walking from last to first instruction, will find all branches

		private void ParseInstructionsBlock(Context context, CodeDomUnparsedExpression expression)
		{
			var from = expression.Instructions.First;
			var to = expression.Instructions.Last;

			var current = from;
			while (current != to)
			{
				Instruction found;
				Instruction target = current.Operand as Instruction;
				var isWhileViaDoWhile = (HasOutgoingJump(current) && (current.OpCode.Code == Code.Br) && 
				                         FindFlowControl(target, to, from, target.Previous, out found, true, FlowControl.Cond_Branch));

				if (current.OpCode.Code == Code.Switch)
				{
					ParseSwitch(context, from, to, expression, current);
				}

				if ((HasIncomingJump(current) && !_processed.Contains(_incoming[current])) || (found != null) ) // Has incoming branch. This means we have "while"
				{
					ParseLoop(context, from, to, expression, current, isWhileViaDoWhile);
					return;
				}

				if (HasOutgoingJump(current) && !_processed.Contains(current)) // Has outgoing branch. This means we have "if-else" statement
				{
					ParseIfElse(context, from, to, expression, current);
					return;
				}
				current = current.Next;
			}
		}

		private void ParseSwitch(Context context, Instruction from, Instruction to, CodeDomExpression expression,
		                         Instruction current)
		{
			var cases = new List<CodeDomCaseExpression>();
			var instructions = current.Operand as Instruction[];
			if (instructions == null || instructions.Length < 2)
			{
				throw new ArgumentException("strange array: < 2");
			}

			// find first non-equal branch
			var index = 0;
			while (instructions[index].Offset == instructions[0].Offset)
				index++;

			// lookup jump from case to the end of switch
			Instruction found = null;
			if (FindFlowControl(current, instructions[index].Previous, instructions[index], to, out found, false))
			{
				CodeDomUnparsedExpression postfix = null;
				var condStart = ResolveStackBlockStart(current);

				// in case we're at end of if-else true block and switch end points out of if-else
				var switchEnd = found.Offset > to.Offset ? to : found.Operand as Instruction;

				// default branch
				Instruction defaultStart = null;
				Instruction defaultEnd;
				CodeDomExpression defaultNode = null;
				if (FindFlowControl(current, instructions[0].Previous, instructions[0], to, out defaultEnd, true))
				{
					_processed.Add(defaultEnd);
					if (defaultEnd.Previous != current)
					{
						defaultStart = current.Next;
						defaultNode = new CodeDomUnparsedExpression(context, defaultStart, defaultEnd);
					}
					else
					{
						if (defaultEnd.Operand != switchEnd)
						{
							defaultStart = defaultEnd.Operand as Instruction;
							defaultEnd = switchEnd.Previous;
							defaultNode = new CodeDomUnparsedExpression(context,  defaultStart, defaultEnd);
						}
						else
						{
							defaultNode = new CodeDomExpression(context);
						}
					}

				}

				// lookup cases
				var groups = instructions.Where(ins => ins != defaultStart).Select((ins, ind) => new {Instruction = ins, Index = ind})
					.GroupBy(gr => gr.Instruction.Offset);

				foreach (var @group in groups)
				{
					var blockStart = @group.First().Instruction;

					if (blockStart != switchEnd)
					{
						// lookup case block end
						var blockEnd = blockStart;
						while ((blockEnd.OpCode.Code != Code.Br && (blockEnd.Operand != switchEnd)) &&
						       blockEnd.Next != switchEnd)
						{
							blockEnd = blockEnd.Next;
						}
						_processed.Add(blockEnd);
						var block = new CodeDomCaseExpression(context, group.Select(g => g.Index).ToArray(),
						                                      new CodeDomUnparsedExpression(context, blockStart, blockEnd));
						cases.Add(block);
					}
				}
				var switchGroup = new CodeDomGroupExpression(context);
				if (from != condStart)
				{
					switchGroup.Add(new CodeDomUnparsedExpression(context, from, condStart.Previous));
				}

				var conditionNode = new CodeDomUnparsedExpression(context, condStart, current);

				switchGroup.Add(new CodeDomSwitchExpression(context, current, conditionNode, defaultNode, cases.ToArray()));

				if (to != switchEnd)
				{
					switchGroup.Add(postfix = new CodeDomUnparsedExpression(context, switchEnd, to));
				}
				expression.ReplaceWith(switchGroup);

				foreach (var @case in cases)
				{
					ParseInstructionsBlock(context, @case.Body as CodeDomUnparsedExpression);
				}
				if (postfix != null) ParseInstructionsBlock(context, postfix);
				if (defaultNode is CodeDomUnparsedExpression) ParseInstructionsBlock(context, defaultNode as CodeDomUnparsedExpression);
				return;
			}

			// else
			throw new ArgumentException("Bad switch block");
		}

		private void ParseLoop(Context context, Instruction from, Instruction to, CodeDomUnparsedExpression expression, Instruction current, bool doWhileJump)
		{
			var group = new CodeDomGroupExpression(context);
			CodeDomUnparsedExpression prefix = null, condition, body, postfix = null;
			LoopType looptype;

			// we have while(){ ... } which is made via do{ ... } while(); template
			if (doWhileJump)
			{ 
				// @current points to unconditional jump to condition block.
				// condition block is placed after body, like in do .. while.
				if(from.Offset < current.Offset)
					prefix = new CodeDomUnparsedExpression(context, from, current.Previous);

				body = new CodeDomUnparsedExpression(context , current.Next , (current.Operand as Instruction).Previous);
				var conditionEnd = _incoming[body.Instructions.First];
				condition = new CodeDomUnparsedExpression(context , body.Instructions.Last.Next , conditionEnd);

				if(conditionEnd.Offset < to.Offset)
					postfix = new CodeDomUnparsedExpression(context, conditionEnd.Next, to);

				_processed.Add(conditionEnd);
				_processed.Add(current);

				looptype = LoopType.While;
			}
			// if loop ends with unconditional branch, we have "while(<condition>) { <body> }"
			else
			{
				var incoming = _incoming[current];
				_processed.Add(incoming);
				if (incoming.OpCode.FlowControl == FlowControl.Branch)
				{
					Instruction conditionEnd;
					if (!FindFlowControl(current, incoming, incoming.Next, to, out conditionEnd, true, FlowControl.Cond_Branch))
					{
						throw new ArgumentException("looks like loop with no exit");
					}

					var conditionStart = ResolveStackBlockStart(conditionEnd);

					if(from.Offset < conditionStart.Offset)
						prefix = new CodeDomUnparsedExpression(context, from, conditionStart.Previous);

					condition = new CodeDomUnparsedExpression(context, conditionStart, conditionEnd);
					body = new CodeDomUnparsedExpression(context, conditionEnd.Next, incoming);

					if(incoming.Offset < to.Offset)
						postfix = new CodeDomUnparsedExpression(context, incoming.Next, to);

					looptype = LoopType.While;
				}
				// otherwice we have " do { <body> } while(<condition>); "
				else
				{
					var conditionStart = ResolveStackBlockStart(incoming);

					if(from.Offset < current.Offset)
						prefix = new CodeDomUnparsedExpression(context, from, current.Previous);

					condition = new CodeDomUnparsedExpression(context, conditionStart, incoming);
					body = new CodeDomUnparsedExpression(context, current, conditionStart.Previous);

					if(incoming.Offset < to.Offset)
						postfix = new CodeDomUnparsedExpression(context, incoming.Next, to);

					looptype = LoopType.DoWhile;
				}
			}

			var condinstruction = condition.Instructions.Last;
			if(prefix != null) group.Add(prefix);
			group.Add(new CodeDomLoopExpression(context, doWhileJump ? current : null, condinstruction, looptype, condition, body));
			if(postfix != null) group.Add(postfix);
			expression.ReplaceWith(group);

			// run for each subblock
			ParseInstructionsBlock(context, body);
			if(postfix != null) {
				ParseInstructionsBlock(context, postfix);
			}
			return;
		}

		private void ParseIfElse(Context context, Instruction from, Instruction to, CodeDomUnparsedExpression expression, Instruction current)
		{
			_processed.Add(current);
			var group = new CodeDomGroupExpression(context);
			var target = (current.Operand as Instruction);
			var condStarts = ResolveStackBlockStart(current);

			// Prefix before condition
			if (condStarts != from)
			{
				group.Add(new CodeDomUnparsedExpression(context, from, condStarts.Previous));
			}

			// condition block = condition except branching instruction
			var conditionNode = new CodeDomUnparsedExpression(context, condStarts, current);
			var falseStart = current.Next;
			var falseEnd = target.Previous;
			var trueStart = target;
			var trueEnd = to;

			Instruction uncondJump;
			CodeDomExpression trueNode, falseNode, postfixNode;

			if (FindFlowControl(falseStart, falseEnd, trueStart, trueEnd, out uncondJump, isforward: false))
			{
				trueEnd = (uncondJump.Operand as Instruction).Previous;
				falseNode = new CodeDomUnparsedExpression(context, falseStart, falseEnd);	// "false" branch
				trueNode = new CodeDomUnparsedExpression(context, trueStart, trueEnd);		// "true" branch
				postfixNode = new CodeDomUnparsedExpression(context, trueEnd.Next, to);		// out of "if-else" block
				_processed.Add(uncondJump);
			}
			else
			{
				falseNode = new CodeDomUnparsedExpression(context, falseStart, falseEnd);	// "false" branch
				trueNode = new CodeDomExpression(context);									// "true" branch
				postfixNode = new CodeDomUnparsedExpression(context, trueStart, trueEnd);	// out of "if-else" block
			}

			if (current.OpCode.Code == Code.Brfalse)
			{
				var tmp = trueNode;
				trueNode = falseNode;
				falseNode = tmp;
			}

			var ifelse = /*((current.OpCode.Code == Code.Brtrue) || (current.OpCode.Code == Code.Brfalse)) ?*/
				new CodeDomBooleanBranchExpression(context, current, 
					conditionNode, 
					new CodeDomGroupExpression(context) { trueNode }, 
					new CodeDomGroupExpression(context) { falseNode }
				);
			group.Add(ifelse);
			group.Add(new CodeDomGroupExpression(context){ postfixNode });
			expression.ReplaceWith(group);

			// run for each subblock

			var unparsedTrueNode = trueNode as CodeDomUnparsedExpression;
			if(unparsedTrueNode != null) ParseInstructionsBlock(context, unparsedTrueNode);

			var unparsedFalseNode = falseNode as CodeDomUnparsedExpression;
			if(unparsedFalseNode != null) ParseInstructionsBlock(context, unparsedFalseNode);

			ParseInstructionsBlock(context, postfixNode as CodeDomUnparsedExpression);

			return;
		}


		#endregion

		#region tools

		private void CalculateIncomingJumps(MethodBody body)
		{
			_incoming.Clear();
			foreach (var instruction in body.Instructions.Where(HasOutgoingJump))
			{
				_incoming[(instruction.Operand as Instruction)] = instruction;
			}
		}

		private bool HasIncomingJump(Instruction position)
		{
			return _incoming.ContainsKey(position);
		}

		private bool HasOutgoingJump(Instruction position)
		{
			switch (position.OpCode.Code)
			{
				case Code.Br:		// Unconditional
				case Code.Brfalse:	// if false, 0, null
				case Code.Brtrue:	// if true, <>0, <>null
				case Code.Beq:		// if 2 values equal
				case Code.Bge:		// if first >= second
				case Code.Bgt:		// if first > second
				case Code.Ble:		// if first <= second
				case Code.Blt:		// if first < second
				case Code.Bne_Un:	// if unsigned1 != unsigned2
				case Code.Bge_Un:	// if unsigned >= unsigned2
				case Code.Bgt_Un:	// if unsigned > unsigned2
				case Code.Ble_Un:	// if unsigned <= unsigned2
				case Code.Blt_Un:	// if unsigned < unsigned2
					return true;
				default:
					return false;
			}
		}	

		private Instruction ResolveStackBlockStart(Instruction to)
		{
			Instruction current = to;
			var stack_delta = 0;
			do
			{
				stack_delta += current.StackDelta();
				current = current.Previous;
			} while(stack_delta != 0);

			return current.Next;
		}

		private bool FindFlowControl(Instruction fromStart, Instruction fromEnd, 
		                             Instruction toStart, Instruction toEnd, 
		                             out Instruction found, 
		                             bool isforward,
		                             FlowControl flowcontrol = FlowControl.Branch)
		{
			bool starts = false;
			var cur = isforward ? fromStart : fromEnd;
			var end = isforward ? fromEnd : fromStart;
			found = null;

			while (cur != end)
			{
				if (starts)
				{
					cur = isforward ? cur.Next : cur.Previous;
				}
				starts = true;
				if(cur.OpCode.FlowControl == flowcontrol)
				{
					var offset = (cur.Operand as Instruction).Offset;
					if(toStart.Offset <= offset && offset <= toEnd.Offset)
					{
						found = cur;
						return true;
					}
				}
			} 
			return false;
		}

		#endregion
	}
}