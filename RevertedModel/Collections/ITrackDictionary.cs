using System;
using System.Collections.Generic;
using System.Text;

namespace RevertedModel.Collections
{
	public interface ITrackDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ITrackObject
	{
	}
}
