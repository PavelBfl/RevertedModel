using RevertedModel.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace RevertedModel.Collections
{
	class CollectionUniqueExecutor<T> : CommandExecutorTarger<TrackCollectionUnique<T>, CollectionUniqueCommand<T>>
	{
		public CollectionUniqueExecutor(TrackCollectionUnique<T> target, CollectionUniqueCommand<T> command)
			: base(target, command)
		{
		}

		protected override void ExecuteForce()
		{
			switch (Command.Action)
			{
				case CollectionUniqueChanged.Add: Target.Add(Command.Item); break;
				case CollectionUniqueChanged.Remove: Target.Remove(Command.Item); break;
				default: throw new InvalidEnumException();
			}
		}

		protected override void UndoForce()
		{
			switch (Command.Action)
			{
				case CollectionUniqueChanged.Add: Target.Remove(Command.Item); break;
				case CollectionUniqueChanged.Remove: Target.Add(Command.Item); break;
				default: throw new InvalidEnumException();
			}
		}
	}
}
