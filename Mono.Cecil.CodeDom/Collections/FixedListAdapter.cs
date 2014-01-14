using System.Collections;
using System.Collections.Generic;

namespace Mono.Cecil.CodeDom.Collections
{
	public class FixedListAdapter<T> : IFixedList<T>
	{
		private readonly IList<T> _items;

		public FixedListAdapter(IList<T> list)
		{
			_items = list;
		}

		public int IndexOf(T item)
		{
			return _items.IndexOf(item);
		}

		#region Implementation of IEnumerable

		public IEnumerator<T> GetEnumerator()
		{
			return _items.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion

		#region Implementation of IReadOnlyList<T>

		public T this[int index]
		{
			get { return _items[index]; }
			set { _items[index] = value; }
		}

		public int Count
		{
			get { return _items.Count; }
		}

		#endregion
	}
}
