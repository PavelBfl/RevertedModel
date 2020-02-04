using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevertedModel.Collections
{
	public class TrackList<T> : TrackCollectionByKey<int, T>, IList<T>
	{
		public TrackList(TrackDispatcher trackDispatcher)
			: base(trackDispatcher)
		{

		}

		private readonly List<T> items = new List<T>();

		public int Count => items.Count;

		public bool IsReadOnly => ((IList<T>)items).IsReadOnly;

		protected override T GetValue(int key)
		{
			return items[key];
		}

		protected override void UpdateForce(int key, T value)
		{
			items[key] = value;
		}

		protected override void InsertForce(int key, T value)
		{
			items.Insert(key, value);
		}

		protected override void RemoveForce(int key)
		{
			items.RemoveAt(key);
		}

		public int IndexOf(T item)
		{
			return items.IndexOf(item);
		}

		public void RemoveAt(int index)
		{
			Remove(index);
		}

		public void Add(T item)
		{
			Insert(Count, item);
		}

		public void Clear()
		{
			while (this.Any())
			{
				Remove(Count - 1);
			}
		}

		public bool Contains(T item)
		{
			return items.Contains(item);
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			items.CopyTo(array, arrayIndex);
		}

		public bool Remove(T item)
		{
			var index = items.IndexOf(item);
			if (index >= 0)
			{
				RemoveAt(index);
				return true;
			}
			return false;
		}

		public IEnumerator<T> GetEnumerator()
		{
			return ((IList<T>)items).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IList<T>)items).GetEnumerator();
		}
	}
}
