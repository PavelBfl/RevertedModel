using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevertedModel.Collections
{
	public abstract class TrackCollectionByKey<TKey, TValue> : TrackObject
	{
		public TrackCollectionByKey(TrackDispatcher trackDispatcher)
			: base(trackDispatcher)
		{
		}

		public TValue this[TKey key]
		{
			get => GetValue(key);
			set
			{
				if (TrackDispatcher.IsEnable)
				{
					TrackDispatcher.AddAndExecute(CollectionByKeyExecutor<TKey, TValue>.Update(this, key, value));
				}
				else
				{
					UpdateForce(key, value);
				}
			}
		}
		protected abstract TValue GetValue(TKey key);
		protected abstract void UpdateForce(TKey key, TValue value);

		public void Insert(TKey key, TValue value)
		{
			if (TrackDispatcher.IsEnable)
			{
				TrackDispatcher.AddAndExecute(CollectionByKeyExecutor<TKey, TValue>.Insert(this, key, value));
			}
			else
			{
				InsertForce(key, value);
			}
		}
		protected abstract void InsertForce(TKey key, TValue value);

		public void Remove(TKey key)
		{
			if (TrackDispatcher.IsEnable)
			{
				TrackDispatcher.AddAndExecute(CollectionByKeyExecutor<TKey, TValue>.Remove(this, key));
			}
			else
			{
				RemoveForce(key);
			}
		}
		protected abstract void RemoveForce(TKey key);
	}
}
