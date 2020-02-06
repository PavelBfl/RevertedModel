using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevertedModel.Collections
{
	public abstract class TrackCollection<T> : TrackObject
	{
		public TrackCollection()
			: base()
		{

		}
		public TrackCollection(TrackDispatcher trackDispatcher)
			: base(trackDispatcher)
		{

		}

	}
}
