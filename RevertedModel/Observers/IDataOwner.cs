using System;
using System.Collections.Generic;
using System.Text;

namespace RevertedModel.Observers
{
	public interface IDataOwner<TParams, TValue>
	{
		void SetData(TParams @params, TValue value);
		TValue GetData(TParams @params);
	}
}
