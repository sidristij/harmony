using System;
using System.Collections.Generic;
using System.Linq;

namespace Mono.Cecil.CodeDom.Collections
{
	internal class MergedList<T> : IFixedList<T> where T : class
	{
		private readonly IList<IFixedList<T>> _lists = new List<IFixedList<T>>();

		public void AddList(IFixedList<T> list)
		{
			_lists.Add(list);
		}

		public IFixedList<T> GetListByIndex(int outindex, out int index)
		{
			int cur = 0;
			foreach (var list in _lists)
			{
				if (outindex < cur + list.Count)
				{
					index = outindex - cur;
					return list;
				}
				cur += list.Count;
			}
			throw new IndexOutOfRangeException("i");
		}

		public int IndexOf(T item)
		{
			int cur = 0;
			foreach (var list in _lists)
			{
				var inner = list.IndexOf(item);
				if (inner >= 0)
				{
					return cur + inner;
				}
				cur += list.Count;
			}
			return -1;
		}


		public T this[int index]
		{
			get
			{
				int listindex;
				var list = GetListByIndex(index, out listindex);
				return list[listindex];
			}
			set
			{
				int listindex;
				var list = GetListByIndex(index, out listindex);
				list[listindex] = value;
			}
		}

		public int Count
		{
			get
			{
				return _lists.Sum(i => i.Count);
			}
		}

		public IEnumerator<T> GetEnumerator()
		{
			return _lists.SelectMany(list => list).GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
