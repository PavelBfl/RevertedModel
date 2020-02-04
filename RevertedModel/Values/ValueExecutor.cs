using System;
using System.Collections.Generic;
using System.Text;

namespace RevertedModel
{
	class ValueExecutor<T> : CommandExecutorTarger<TrackValue<T>, ValueCommand<T>>
	{
		public ValueExecutor(TrackValue<T> target, ValueCommand<T> command)
			: base(target, command)
		{
			OldValue = Target.Value;
		}

		public T OldValue { get; } = default;

		protected override void ExecuteForce()
		{
			Target.Value = Command.NewValue;
		}

		protected override void UndoForce()
		{
			Target.Value = OldValue;
		}
	}
}
