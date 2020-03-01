using System;
using System.Collections.Generic;
using System.Text;

namespace RevertedModel.Collections
{
	public interface ITrackSet<T> : ICollection<T>, ITrackObject
	{
	}
}
