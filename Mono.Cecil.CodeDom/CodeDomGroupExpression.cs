using System.Collections.Generic;
using Mono.Cecil.CodeDom.Collections;

namespace Mono.Cecil.CodeDom
{
	public class CodeDomGroupExpression : CodeDomExpression, IList<CodeDomExpression>
	{
		private readonly List<CodeDomExpression> _list;

		public CodeDomGroupExpression(Context context) : base(context)
		{
			_list = new List<CodeDomExpression>();
			Nodes = new FixedListAdapter<CodeDomExpression>(_list);
		}

		/// <inheritdoc/>
		public override bool IsGroup
		{
			get { return true; }
		}

		/// <inheritdoc/>
		public override bool IsExtandableGroup
		{
			get { return true; }
		}

		public override bool IsEmpty
		{
			get
			{
				return Nodes.Count == 0;
			}
		}

		#region IList implementation

		public int IndexOf(CodeDomExpression item)
		{
			item.ParentNode = this;
			return _list.IndexOf(item);
		}

		public void Insert(int index, CodeDomExpression item)
		{
			item.ParentNode = this;
			_list.Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			var item = this[index];
			_list.RemoveAt(index);
			item.ParentNode = null;
		}

		public CodeDomExpression this[int index]
		{
			get
			{
				return _list[index];
			}
			set
			{
				value.ParentNode = this;
				_list[index] = value;
			}
		}

		#endregion

		#region ICollection implementation

		public void Add(CodeDomExpression item)
		{
			item.ParentNode = this;
			_list.Add(item);
		}

		public void Clear()
		{
			_list.Clear();
		}

		public bool Contains(CodeDomExpression item)
		{
			return _list.Contains(item);
		}

		public void CopyTo(CodeDomExpression[] array, int arrayIndex)
		{
			_list.CopyTo(array, arrayIndex);
		}

		public bool Remove(CodeDomExpression item)
		{
			item.ParentNode = null;
			return _list.Remove(item);
		}

		public int Count
		{
			get
			{
				return _list.Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		#endregion

		public override string ToString()
		{
			return string.Join(System.Environment.NewLine, Nodes);
		}
	}
}

