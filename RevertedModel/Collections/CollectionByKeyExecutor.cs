using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace RevertedModel.Collections
{
	class CollectionByKeyExecutor<TKey, TValue> : CommandExecutorTarger<CommandedCollectionByKey<TKey, TValue>, CollectionByKeyCommand<TKey, TValue>>
	{
		public CollectionByKeyExecutor(CommandedCollectionByKey<TKey, TValue> target, CollectionByKeyCommand<TKey, TValue> command)
			: base(target, command)
		{
			if (command.Action == CollectionByKeyChanged.Remove || command.Action == CollectionByKeyChanged.Update)
			{
				OldValue = target[command.Key];
			}
		}

		public static CollectionByKeyExecutor<TKey, TValue> Insert(CommandedCollectionByKey<TKey, TValue> target, TKey key, TValue newValue)
		{
			return new CollectionByKeyExecutor<TKey, TValue>(target, new CollectionByKeyCommand<TKey, TValue>(CollectionByKeyChanged.Insert, key, newValue));
		}
		public static CollectionByKeyExecutor<TKey, TValue> Remove(CommandedCollectionByKey<TKey, TValue> target, TKey key)
		{
			return new CollectionByKeyExecutor<TKey, TValue>(target, new CollectionByKeyCommand<TKey, TValue>(CollectionByKeyChanged.Remove, key, default));
		}
		public static CollectionByKeyExecutor<TKey, TValue> Update(CommandedCollectionByKey<TKey, TValue> target, TKey key, TValue newItem)
		{
			return new CollectionByKeyExecutor<TKey, TValue>(target, new CollectionByKeyCommand<TKey, TValue>(CollectionByKeyChanged.Update, key, newItem));
		}

		public TValue OldValue { get; } = default;

		protected override void ExecuteForce()
		{
			switch (Command.Action)
			{
				case CollectionByKeyChanged.Insert: Target.Insert(Command.Key, Command.NewValue); break;
				case CollectionByKeyChanged.Remove: Target.Remove(Command.Key); break;
				case CollectionByKeyChanged.Update: Target[Command.Key] = Command.NewValue; break;
				default: throw new InvalidEnumArgumentException();
			}
		}

		protected override void UndoForce()
		{
			switch (Command.Action)
			{
				case CollectionByKeyChanged.Insert: Target.Remove(Command.Key); break;
				case CollectionByKeyChanged.Remove: Target.Insert(Command.Key, OldValue); break;
				case CollectionByKeyChanged.Update: Target[Command.Key] = OldValue; break;
				default: throw new InvalidEnumArgumentException();
			}
		}
	}
}
