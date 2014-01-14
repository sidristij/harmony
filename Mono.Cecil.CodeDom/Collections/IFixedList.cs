using System.Collections.Generic;

namespace Mono.Cecil.CodeDom.Collections
{
	public interface IFixedList<T> : IEnumerable<T>
	{
		T this[int index] { get; set; }
		int Count { get; }
		int IndexOf(T item);
	}
}
