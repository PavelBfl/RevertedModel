using MockGenerators;
using RevertedModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace RevertedModelTests.Collections
{
	class CommandedListTests<T>
	{
		public CommandedListTests(IValueGenerator<T> itemsGenerator)
		{
			ItemsGenerator = itemsGenerator ?? throw new NullReferenceException(nameof(itemsGenerator));
		}

		public IValueGenerator<T> ItemsGenerator { get; } = null;
		public CommandDispatcher CommandDispatcher { get; } = new CommandDispatcher(new DefaultOffsetTokenDispatcher());
	}
}
