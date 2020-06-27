using System;
using System.Collections.Generic;
using System.Text;

namespace RevertedModel.Observers
{
	public class ObserverExecutor<TParams, TValue> : CommandExecutor<ObserverCommand<TParams, TValue>>
	{
		public ObserverExecutor(IDataOwner<TParams, TValue> dataOwner, ObserverCommand<TParams, TValue> command)
			: base(command)
		{
			DataOwner = dataOwner ?? throw new ArgumentNullException(nameof(dataOwner));
			OldValue = DataOwner.GetData(Command.Params);
		}
		
		private IDataOwner<TParams, TValue> DataOwner { get; }
		private TValue OldValue { get; }

		public override void Execute()
		{
			DataOwner.SetData(Command.Params, Command.Value);
		}

		public override void Undo()
		{
			DataOwner.SetData(Command.Params, OldValue);
		}
	}
}
