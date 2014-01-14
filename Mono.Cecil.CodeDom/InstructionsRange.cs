using System.Collections.Generic;
using Mono.Cecil.Cil;

namespace Mono.Cecil.CodeDom
{
	public class InstructionsRange : IEnumerable<Instruction>
	{
		public Instruction First { get; internal set; }

		public Instruction Last { get; internal set; }

		#region IEnumerable implementation

		public IEnumerator<Instruction> GetEnumerator()
		{
			var current = First;
			var iterator = current;
			do {
				current = iterator;
				yield return current;
				iterator = iterator.Next;
			} while(current != Last);
		}

		#endregion

		#region IEnumerable implementation

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion
	}

}
