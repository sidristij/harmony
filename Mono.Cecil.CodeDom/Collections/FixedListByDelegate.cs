using System;
using System.Collections;
using System.Collections.Generic;

namespace Mono.Cecil.CodeDom.Collections
{
	internal class FixedListByDelegate<T> : IFixedList<T> where T : class
	{
		private Func<int, T> _getter;
		private Action<int, T> _setter;
		private Func<int> _count;

		public FixedListByDelegate(Func<int, T> getter, Action<int, T> setter, Func<int> count)
		{
			_getter = getter;
			_setter = setter;
			_count = count;
		}

		public int IndexOf(T item)
		{
			for (int i=0, len=_count(); i<len; i++)
			{
				if (_getter(i) == item)
					return i;
			}
			return -1;
		}

		#region Implementation of IEnumerable

		public IEnumerator<T> GetEnumerator()
		{
			for(int i = 0, len = _count(); i < len; i++)
			{
				yield return _getter(i);
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion

		#region Implementation of IFixedList<T>

		public T this[int index]
		{
			get { return _getter(index); }
			set { _setter(index, value); }
		}

		public int Count
		{
			get { return _count(); }
		}

		#endregion
	}
}
