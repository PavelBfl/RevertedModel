using System;
using System.Collections.Generic;
using System.Text;

namespace RevertedModel.Observers
{
	public class ObserverCommand<TParams, TValue> : Command
	{
		public ObserverCommand(TParams @params, TValue value)
		{
			Params = @params;
			Value = value;
		}
		public ObserverCommand(TValue value)
			: this(default, value)
		{
			
		}

		public TParams Params { get; }
		public TValue Value { get; }
	}
}
