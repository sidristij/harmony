using System;
using System.Collections.Generic;

namespace Mono.Cecil.CodeDom.Collections
{
	public class FixedList<T> : IFixedList<T> where T : class
	{
		private readonly List<T> _list = new List<T>();

		private readonly Func<T> _generator;

		public FixedList(Func<T> generator, int capacity = 0)
		{
			_generator = generator;
			ExtendToSize(capacity);
		}

		public FixedList(int capacity = 0) : this( () => default(T), capacity)
		{
		}

		public static FixedList<T> Empty = new FixedList<T>();

		public T this[int index]
		{
			get
			{
				return _list[index];
			}
			set
			{
				ExtendToSize(index);
				_list[index] = value;
			}
		}

		public int IndexOf(T item)
		{
			return _list.IndexOf(item);
		}

		public int Count
		{
			get
			{
				return _list.Count;
			}
		}

		public IEnumerator<T> GetEnumerator()
		{
			foreach (var item in _list)
			{
				if (item != null)
				{
					yield return item;
				}
			}
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return _list.GetEnumerator();
		}

		private void ExtendToSize(int size)
		{
			for (int i=Count; i < size; i++)
			{
				_list.Add(_generator());
			}
		}

	}
}

