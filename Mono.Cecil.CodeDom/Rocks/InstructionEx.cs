using System;
using Mono.Cecil.Cil;

namespace Mono.Cecil.CodeDom
{
	public static class InstructionEx
	{
		/// <summary>
		/// Computes the given instruction stack delta and adds this delta to given stack_size property. If forward property is false, stack delta will be with minus sign.
		/// </summary>
		/// <param name="instruction">Instruction to inspect.</param>
		/// <param name="forward">Indicates that we have forward direction.</param>
		public static int StackDelta (this Instruction instruction, bool forward = false)
		{
			int stack_size = 0;
			switch (instruction.OpCode.FlowControl) {
				case FlowControl.Call:
				{
					var method = (IMethodSignature) instruction.Operand;

					// pop 'this' argument
					if (method.HasThis && instruction.OpCode.Code != Code.Newobj)
						stack_size += forward ? -1 : 1;

					// pop normal arguments
					if (method.HasParameters)
						stack_size += forward ? -method.Parameters.Count : method.Parameters.Count;

					// pop function pointer
					if (instruction.OpCode.Code == Code.Calli)
						stack_size += forward ? -1 : 1;

					// push return value
					if (method.ReturnType.MetadataType != MetadataType.Void || instruction.OpCode.Code == Code.Newobj)
						stack_size += forward ? 1 : -1;
					break;
				}
					default:
				{
					ComputePopDelta(instruction.OpCode.StackBehaviourPop, forward, ref stack_size);
					ComputePushDelta(instruction.OpCode.StackBehaviourPush, forward, ref stack_size);
					break;
				}
			}
			return stack_size;
		}

		static void ComputePopDelta (StackBehaviour pop_behavior, bool forward, ref int stack_size)
		{
			switch (pop_behavior) {
				case StackBehaviour.Popi:
					case StackBehaviour.Popref:
					case StackBehaviour.Pop1:
					stack_size += forward ? -1 : 1;
					break;

					case StackBehaviour.Pop1_pop1:
					case StackBehaviour.Popi_pop1:
					case StackBehaviour.Popi_popi:
					case StackBehaviour.Popi_popi8:
					case StackBehaviour.Popi_popr4:
					case StackBehaviour.Popi_popr8:
					case StackBehaviour.Popref_pop1:
					case StackBehaviour.Popref_popi:
					stack_size += forward ? -2 : 2;
					break;

					case StackBehaviour.Popi_popi_popi:
					case StackBehaviour.Popref_popi_popi:
					case StackBehaviour.Popref_popi_popi8:
					case StackBehaviour.Popref_popi_popr4:
					case StackBehaviour.Popref_popi_popr8:
					case StackBehaviour.Popref_popi_popref:
					stack_size += forward ? -3 : 3;
					break;

					case StackBehaviour.PopAll:
					stack_size = 0;
					break;
			}
		}

		static void ComputePushDelta (StackBehaviour push_behaviour, bool forward, ref int stack_size)
		{
			switch (push_behaviour)
			{
				case StackBehaviour.Push1:
					case StackBehaviour.Pushi:
					case StackBehaviour.Pushi8:
					case StackBehaviour.Pushr4:
					case StackBehaviour.Pushr8:
					case StackBehaviour.Pushref:
					stack_size += forward ? 1 : -1;
					break;

					case StackBehaviour.Push1_push1:
					stack_size += forward ? 2 : -2;
					break;
			}
		}
	}
}

