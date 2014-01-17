using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil.Cil;
using Mono.Cecil.CodeDom.Parser.Members;
using Mono.Cecil.CodeDom.Parser.Tcf;

namespace Mono.Cecil.CodeDom.Parser
{
	public class CodeDomParser : CodeDomParserBase
	{
		private Stack<CodeDomExpression> _stack = new Stack<CodeDomExpression>();

		private CodeDomExpression PushToStack(CodeDomSingleInstructionExpression node)
		{
			_stack.Push(node);
			return node;
		}

		public CodeDomExpression Parse(MethodDefinition def_method, Instruction start, Instruction end = null, CatchBlockExpression catchBlock = null)
		{
			var current = start;
			var last = end == null ? null : end.Next;
			var context = new Context(def_method, this);
			var parsedNodes = new List<CodeDomExpression>();
			CodeDomExpression exceptionVariable = null;

			if (catchBlock != null)
			{
				exceptionVariable = CodeDom.CatchPush(context, catchBlock.Test);
				_stack.Push(exceptionVariable);
			}

			// While we have instruction and instruction is not covered yet
			while (current != last && current != null)
			{
				switch (current.OpCode.Code)
				{

					case Code.Nop:
					{
						current = current.Next;
						break;
					}

					#region Loading

					/* Parameter -> Stack */
					case Code.Ldarg:
					//case Code.Ldarga:
					{
						PushToStack(CodeDom.ParameterGet(context, current, (ParameterDefinition)current.Operand));
						current = current.Next;
						break;
					}

					/* Field -> Stack */
					case Code.Ldfld:
					{
						var instance = _stack.Pop();
						var field = (FieldReference)current.Operand;
						PushToStack(CodeDom.FieldGet(context, current, field, instance));
						current = current.Next;
						break;
					}

					case Code.Ldsfld:
					{
						var field = (FieldReference)current.Operand;
						PushToStack(CodeDom.FieldGet(context, current, field));
						current = current.Next;
						break;
					}

					/* Field -> Stack */
					case Code.Ldflda:
					{
						var field = (FieldReference)current.Operand;
						PushToStack(CodeDom.FieldGetRef(context, current, field));
						current = current.Next;
						break;
					}

					case Code.Ldsflda:
					{
						var instance = _stack.Pop();
						var field = (FieldReference)current.Operand;
						PushToStack(CodeDom.FieldGetRef(context, current, field, instance));
						current = current.Next;
						break;
					}

					/* Constant (Int32) -> Stack */
					case Code.Ldc_I4:
					{
						PushToStack(CodeDom.Push(context, current, Convert.ToInt32(current.Operand)));
						current = current.Next;
						break;
					}

					/* Constant (Int64) -> Stack */
					case Code.Ldc_I8:
					{
						PushToStack(CodeDom.Push(context, current, Convert.ToInt64(current.Operand)));
						current = current.Next;
						break;
					}

					/* Constant (float) -> Stack */
					case Code.Ldc_R4:
					{
						PushToStack(CodeDom.Push(context, current, Convert.ToSingle(current.Operand)));
						current = current.Next;
						break;
					}

					/* Constant (double) -> Stack */
					case Code.Ldc_R8:
					{
						PushToStack(CodeDom.Push(context, current, Convert.ToDouble(current.Operand)));
						current = current.Next;
						break;
					}

					/* String -> Stack */
					case Code.Ldstr:
					{
						PushToStack(CodeDom.Push(context, current, (string)current.Operand));
						current = current.Next;
						break;
					}

					/* Variable -> Stack */
					case Code.Ldloc:
					case Code.Ldloca:
					{
						PushToStack(CodeDom.VariableGet(context, current, (VariableReference)current.Operand));
						current = current.Next;
						break;
					}

					/* null -> Stack */
					case Code.Ldnull:
					{
						PushToStack(CodeDom.PushNull(context, current));
						current = current.Next;
						break;
					}

					/* Stack -> Variable */
					case Code.Stloc:
					{
						parsedNodes.Add(CodeDom.VariableSet(context, current, (VariableReference)current.Operand, exp_value: _stack.Pop()));
						current = current.Next;
						break;
					}

					/* new SomeType( ... ) */
					case Code.Newobj:
					{
						var ctorMethod = (MethodReference)current.Operand;
						var methodParams = ctorMethod.Parameters.Select(t => _stack.Pop()).ToList();
						methodParams.Reverse();
						PushToStack(CodeDom.CreateObject(context, current, ctorMethod, methodParams.ToArray()));

						current = current.Next;
						break;
					}

					/* new array[] { } */
					case Code.Newarr:
					{
						var ref_type = (TypeReference)current.Operand;
						PushToStack(CodeDom.CreateArray(context, current, ref_type, exp_length: _stack.Pop()));
						current = current.Next;
						break;

					}

					/* array.Length */
					case Code.Ldlen:
					{
						var ref_type = (TypeReference)current.Operand;
						PushToStack(CodeDom.ArrayLength(context, current, ref_type, exp_array: _stack.Pop()));
						current = current.Next;
						break;
					}

					#endregion

					#region branching


					case Code.Brfalse:  // if false, 0, null
					case Code.Brtrue:   // if true, <>0, <>null
					{
						var exp_condition = new Mono.Cecil.CodeDom.Parser.Branching.CodeDomBoolConditionExpression(context, _stack.Pop());  
						//_solved.Add(current, exp_condition);
						parsedNodes.Add(exp_condition);
						current = current.Next;
						break;
					}

					case Code.Br:       // Unconditional
					case Code.Beq:      // if 2 values equal
					case Code.Bge:      // if first >= second
					case Code.Bgt:      // if first > second
					case Code.Ble:      // if first <= second
					case Code.Blt:      // if first < second
					case Code.Bne_Un:   // if unsigned1 != unsigned2
					case Code.Bge_Un:   // if unsigned >= unsigned2
					case Code.Bgt_Un:   // if unsigned > unsigned2
					case Code.Ble_Un:   // if unsigned <= unsigned2
					case Code.Blt_Un:   // if unsigned < unsigned2
					{
						var right = _stack.Pop();
						var left = _stack.Pop();
						PushToStack(CodeDom.SimpleExpression(context, current, left, right));
						current = current.Next;
						break;
					}

					#endregion

					#region calling

					case Code.Call:
					case Code.Calli:
					case Code.Callvirt:
					{
						var ref_method = (MethodReference)current.Operand;
						var parametersCount = ref_method.Parameters.Count;

						var methodParams = new CodeDomExpression[parametersCount];
						for(var index = parametersCount - 1; index >= 0; index --)
						{
							methodParams[index] = _stack.Pop();
						}

						var exp_instance = ref_method.HasThis ? _stack.Pop() : new CodeDomExpression(context);

						var exp_call = CodeDom.MethodCall(context, current, ref_method, exp_instance, methodParams.Reverse().ToArray());
						if (!exp_call.IsFinal)
						{
							PushToStack(exp_call);
						}
						else
						{
							//_solved.Add(current, exp_call);
							parsedNodes.Add(exp_call);
						}
						current = current.Next;
						break;
					}
					#endregion

					case Code.Ldelema:      // need to cast
					case Code.Ldelem_Any:   // need to cast
					case Code.Ldelem_Ref:   // need to cast
					case Code.Ldelem_I1:
					case Code.Ldelem_U1:
					case Code.Ldelem_I2:
					case Code.Ldelem_U2:
					case Code.Ldelem_I4:
					case Code.Ldelem_U4:
					case Code.Ldelem_I8:
					case Code.Ldelem_I:
					case Code.Ldelem_R4:
					case Code.Ldelem_R8:
					{
						var index = _stack.Pop();
						var @array = _stack.Pop();
						PushToStack(CodeDom.ArrayGetItem(context, current, @array, index));
						current = current.Next;
						break;
					}

					case Code.Stelem_I:
					case Code.Stelem_I1:
					case Code.Stelem_I2:
					case Code.Stelem_I4:
					case Code.Stelem_I8:
					case Code.Stelem_R4:
					case Code.Stelem_R8:
					case Code.Stelem_Ref:
					case Code.Stelem_Any:
					{
						var @value = _stack.Pop();
						var index = _stack.Pop();
						var @array = _stack.Pop();
						parsedNodes.Add(CodeDom.ArraySetItem(context, current, @array, index, @value));
						current = current.Next;
						break;
					}

					case Code.Sizeof:
					{
						PushToStack(CodeDom.Sizeof(context, current));
						current = current.Next;
						break;
					}

					#region misc

					// Compare'n'push 1 or 0
					case Code.Ceq:
					case Code.Cgt:
					case Code.Cgt_Un:
					case Code.Clt:
					case Code.Clt_Un:

					// Bitwise ops
					case Code.And:
					case Code.Or:
					case Code.Xor:

					// Math ops
					case Code.Add:
					case Code.Sub:
					case Code.Mul:
					case Code.Div:
					case Code.Div_Un:
					case Code.Rem:
					case Code.Rem_Un:
					case Code.Add_Ovf:
					case Code.Add_Ovf_Un:
					case Code.Mul_Ovf:
					case Code.Mul_Ovf_Un:
					case Code.Sub_Ovf:
					case Code.Sub_Ovf_Un:

					// Bitwise shift
					case Code.Shl:
					case Code.Shr:
					case Code.Shr_Un:
					{
						var right = _stack.Pop();
						var left = _stack.Pop();
						PushToStack(CodeDom.SimpleExpression(context, current, left, right));
						current = current.Next;
						break;
					}

					// (int)y, checked { (uint)char_value; }
					case Code.Conv_I1:
					case Code.Conv_I2:
					case Code.Conv_I4:
					case Code.Conv_I8:
					case Code.Conv_R4:
					case Code.Conv_R8:
					case Code.Conv_U4:
					case Code.Conv_U8:
					case Code.Conv_R_Un:
					case Code.Conv_Ovf_I1_Un:
					case Code.Conv_Ovf_I2_Un:
					case Code.Conv_Ovf_I4_Un:
					case Code.Conv_Ovf_I8_Un:
					case Code.Conv_Ovf_U1_Un:
					case Code.Conv_Ovf_U2_Un:
					case Code.Conv_Ovf_U4_Un:
					case Code.Conv_Ovf_U8_Un:
					case Code.Conv_Ovf_I_Un:
					case Code.Conv_Ovf_U_Un:
					case Code.Conv_Ovf_I1:
					case Code.Conv_Ovf_U1:
					case Code.Conv_Ovf_I2:
					case Code.Conv_Ovf_U2:
					case Code.Conv_Ovf_I4:
					case Code.Conv_Ovf_U4:
					case Code.Conv_Ovf_I8:
					case Code.Conv_Ovf_U8:
					case Code.Conv_U2:
					case Code.Conv_U1:
					case Code.Conv_U:
					case Code.Conv_I:
					case Code.Conv_Ovf_I:
					case Code.Conv_Ovf_U:
					{
						PushToStack(CodeDom.CastValue(context, current, exp_instance: _stack.Pop()));
						current = current.Next;
						break;
					}

					// (TType)instance
					case Code.Castclass:
					{
						PushToStack(CodeDom.CastClass(context, current, _stack.Pop()));
						current = current.Next;
						break;
					}

					case Code.Ldind_I1:
					case Code.Ldind_U1:
					case Code.Ldind_I2:
					case Code.Ldind_U2:
					case Code.Ldind_I4:
					case Code.Ldind_U4:
					case Code.Ldind_I8:
					case Code.Ldind_I:
					case Code.Ldind_R4:
					case Code.Ldind_R8:
					case Code.Ldind_Ref:
					{
						PushToStack(CodeDom.PtrCastToPointer(current, ref_instance: _stack.Pop()));
						current = current.Next;
						break;
					}

					case Code.Stind_Ref:
					case Code.Stind_I1:
					case Code.Stind_I2:
					case Code.Stind_I4:
					case Code.Stind_I8:
					case Code.Stind_R4:
					case Code.Stind_R8:
					case Code.Stind_I:
					{
						var @value = _stack.Pop();
						var @address = _stack.Pop();
						parsedNodes.Add(CodeDom.PtrSetValue(current, @address, @value));
						current = current.Next;
						break;
					}

					// (IntPtr)(void *)&Console.WriteLine -> stack
					case Code.Ldftn:
					{
						var ref_method = (MethodReference)current.Operand;
						PushToStack(CodeDom.PtrCastToMethodHandler(context, current, ref_method));
						current = current.Next;
						break;
					}

					// *pIntValue -> stack
					case Code.Ldobj:
					{
						var ref_type = (TypeReference)current.Operand;
						var ptr_instance = _stack.Pop();
						PushToStack(CodeDom.PtrCastToValue(current, ptr_instance, ref_type));
						current = current.Next;
						break;
					}

					// int x; *pIntValue = x;
					case Code.Stobj:
					{
						var ref_type = (TypeReference)current.Operand;
						var exp_sourceValue = _stack.Pop();
						var exp_destinationValue = _stack.Pop();
						parsedNodes.Add(CodeDom.PtrCopyFromToValue(current, ref_type, exp_sourceValue, exp_destinationValue));
						current = current.Next;
						break;
					}

					// *pIntValue = *pIntValue2;
					case Code.Cpobj:
					{
						var ref_type = (TypeReference)current.Operand;
						var ptr_source = _stack.Pop();
						var ptr_destination = _stack.Pop();
						parsedNodes.Add(CodeDom.PtrCopyFromToValue(current, ref_type, ptr_source, ptr_destination));
						current = current.Next;
						break;
					}

					// stack -> <caller>
					case Code.Ret:
					{
						if (_stack.Count > 0)
						{
							parsedNodes.Add(CodeDom.Return(context, current, _stack.Pop()));
						}
						else
						{
							parsedNodes.Add(CodeDom.Return(context, current));
						}

						current = current.Next;
						break;
					}

					// instance is TType
					case Code.Isinst:
					{
						var instance = _stack.Pop();
						PushToStack(CodeDom.IsInstanceOf(context, current, instance));
						current = current.Next;
						break;
					}

					// typeof(..), methodof(..), fieldof(..)
					case Code.Ldtoken:
					{
						PushToStack(CodeDom.LoadMetadataToken(context, current));
						current = current.Next;
						break;
					}

					#endregion

					#region Exceptions
					case Code.Throw:
					{
						var exception = _stack.Pop();
						parsedNodes.Add(CodeDom.Throw(context, current, exception));
						current = current.Next;
						break;
					}

					case Code.Endfinally:
					{
						// skip because we will make this block on code generation
						current = last;
						break;
					}

					case Code.Endfilter:
					{
						// var filter = _stack.Pop(); // 0=exception_continue_search, 1=exception_execute_handler
						// parsedNodes.Add(CodeDom.TcfEndFilter(current, filter));
						// current = last;
						throw new InvalidOperationException("Endfilter opcode found. Should be implemented.˚");
					}

					case Code.Leave:
					{
						current = last;
						break;
					}

					case Code.Ckfinite:
					{
						parsedNodes.Add(CodeDom.Ckfinite(context, current, _stack.Pop()));
						current = current.Next;
						break;
					}
					#endregion

					case Code.Pop:
					{
						var expr = _stack.Pop();
						if (_stack.Count == 0)
						{
							parsedNodes.Add(expr);
						}
						current = current.Next;
						break;
					}

					case Code.Dup:
					{
						_stack.Push((_stack.Peek()));
						current = current.Next;
						break;
					}

					case Code.Neg:
					{
						PushToStack(CodeDom.Neg(context, current, _stack.Pop()));
						current = current.Next;
						break;
					}

					case Code.Not:
					{
						PushToStack(CodeDom.Not(context, current, _stack.Pop()));
						current = current.Next;
						break;
					}

					case Code.Switch:
					{
						// var branches = current.Operand as Instruction[];
						// var branchIndex = (CodeDomExpression)_stack.Pop();
						// parsedNodes.Add(CodeDom.SwitchTable(current, branchIndex, branches));
						current = current.Next;
						break;
					}

					/* IntPtr Alloc(uint bytes_num) */
					case Code.Localloc:
					{
						var allocsize = _stack.Pop();
						PushToStack(CodeDom.PtrMemAlloc(current, allocsize));
						current = current.Next;
						break;
					}

					/* initializes all fields of _struct_ to zeroes or nulls */
					case Code.Initobj:
					{
						var exp_instance = _stack.Pop();
						var ref_type = (TypeReference)current.Operand;
						parsedNodes.Add(CodeDom.InitStruct(context, current, exp_instance, ref_type));
						current = current.Next;
						break;
					}

					case Code.Starg:
					{
						var ref_parameter = (ParameterReference)current.Operand;
						var exp_value = _stack.Pop();
						parsedNodes.Add(CodeDom.ParameterSet(context, current, ref_parameter, exp_value));
						current = current.Next;
						break;
					}

					case Code.Stfld:
					{
						var @value = _stack.Pop();
						var instance = _stack.Pop();
						var ref_field = (FieldReference)current.Operand;
						parsedNodes.Add(CodeDom.FieldSet(context, current, ref_field, value, instance));
						current = current.Next;
						break;
					}

					case Code.Stsfld:
					{
						var @value = _stack.Pop();
						var ref_field = (FieldReference)current.Operand;
						parsedNodes.Add(CodeDom.FieldSet(context, current, ref_field, value));
						current = current.Next;
						break;
					}

					case Code.Cpblk:
					{
						var bytesToCopy = _stack.Pop();
						var source = _stack.Pop();
						var destination = _stack.Pop();
						parsedNodes.Add(CodeDom.PtrMemCopy(current, source, destination, bytesToCopy));
						current = current.Next;
						break;
					}

					case Code.Initblk:
					{
						var bytesnum = _stack.Pop();
						var initvalue = _stack.Pop();
						var address = _stack.Pop();
						parsedNodes.Add(CodeDom.PtrMemInit(current, address, initvalue, bytesnum));
						current = current.Next;
						break;
					}

					case Code.Box:
					{
						PushToStack(CodeDom.Boxing(context, current, _stack.Pop()));
						current = current.Next;
						break;
					}

					case Code.Unbox:
					case Code.Unbox_Any:
					{
						PushToStack(CodeDom.Unboxing(context, current, _stack.Pop()));
						current = current.Next;
						break;
					}

					/* not to be represented in AST */
					case Code.Unaligned:
					case Code.Volatile:
					case Code.Readonly:
					case Code.Break:
					{
						parsedNodes.Add(CodeDom.SkippedInstruction(current));
						current = current.Next;
						break;
					}

					default:
						Console.WriteLine("Uncovered instruction found: {0} on 0x{1:X} ", current.OpCode.Code, current.Offset);
						parsedNodes.Add(CodeDom.UncoveredInstruction(context, current));
						current = current.Next;
						break;
				}
			}

			// If stack contains resulting expression, we couldn't create group for it. Else group should be created
			CodeDomExpression root = _stack.Any() ? _stack.Pop() : new CodeDomGroupExpression(context);

			if (parsedNodes.Any() && !root.IsGroup)
			{
				throw new ParserStateException("Illegal app flow found: trying to push many parsed nodes into non-group node");
			}

			var group = root as CodeDomGroupExpression;

			if (group != null)
			{
				foreach (var node in parsedNodes)
				{
					group.Add(node);
					node.ParentNode = group;
				}
			}

			// resolve catch variable
			if (catchBlock != null && exceptionVariable.ParentNode is VariableSetExpression)
			{
				catchBlock.VariableReference = (exceptionVariable.ParentNode as VariableSetExpression).VariableReference;
			}

			root.ParentNode = catchBlock;
			return root;
		}
	}
}

/*
Strange:

        Jmp,
        Arglist,
        Refanyval,
        Mkrefany,
        Refanytype,
        Tail,
        Constrained,
        Ldvirtftn,
 */
