using System;
using System.Collections.Generic;
using System.Text;

namespace RevertedModel
{
	public class CommandedValuesCreator
	{
		public CommandedValuesCreator(TrackDispatcher commandDispatcher)
		{
			CommandDispatcher = commandDispatcher ?? throw new ArgumentNullException(nameof(commandDispatcher));
		}

		public TrackDispatcher CommandDispatcher { get; } = null;

		public TrackValue<T> Create<T>(T initValue = default)
		{
			return new TrackValue<T>(initValue, CommandDispatcher);
		}
	}
}
