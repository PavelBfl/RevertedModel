using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevertedModel.Collections
{
	public class CommandedCollectionUnique<T> : CommandedObject, ICollection<T>
	{
		public CommandedCollectionUnique(CommandDispatcher commandDispatcher)
			: base(commandDispatcher)
		{
		}

		private readonly HashSet<T> items = new HashSet<T>();

		public int Count => items.Count;

		public bool IsReadOnly => ((ICollection<T>)items).IsReadOnly;

		public void Add(T item)
		{
			if (CommandRecording)
			{
				CommandDispatcher.AddAndExecute(new CollectionUniqueExecutor<T>(this, new CollectionUniqueCommand<T>(CollectionUniqueChanged.Add, item)));
			}
			else
			{
				AddForce(item);
			}
		}
		protected virtual void AddForce(T item)
		{
			items.Add(item);
		}

		public void Clear()
		{
			foreach (var item in items.ToArray())
			{
				Remove(item);
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
			if (CommandRecording)
			{
				CommandDispatcher.AddAndExecute(new CollectionUniqueExecutor<T>(this, new CollectionUniqueCommand<T>(CollectionUniqueChanged.Remove, item)));
				return true;
			}
			else
			{
				return RemoveForce(item);
			}
		}
		protected virtual bool RemoveForce(T item)
		{
			return items.Remove(item);
		}

		public IEnumerator<T> GetEnumerator()
		{
			return ((ICollection<T>)items).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((ICollection<T>)items).GetEnumerator();
		}
	}
}
