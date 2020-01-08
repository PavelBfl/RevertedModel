using System;
using System.Collections.Generic;
using System.Text;

namespace RevertedModel
{
	public class CommandedValuesCreator
	{
		public CommandedValuesCreator(CommandDispatcher commandDispatcher)
		{
			CommandDispatcher = commandDispatcher ?? throw new NullReferenceException(nameof(commandDispatcher));
		}

		public CommandDispatcher CommandDispatcher { get; } = null!;

		public CommandedValue<T> Create<T>(T initValue = default!)
		{
			return new CommandedValue<T>(initValue, CommandDispatcher);
		}
	}
}
