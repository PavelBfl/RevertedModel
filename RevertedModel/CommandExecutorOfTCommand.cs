using System;
using System.Collections.Generic;
using System.Text;

namespace RevertedModel
{
	/// <summary>
	/// Объект выполнения команды
	/// </summary>
	public abstract class CommandExecutor<TCommand> : CommandExecutor where TCommand : Command
	{
		public CommandExecutor(TCommand command)
		{
			Command = command ?? throw new ArgumentNullException(nameof(command));
		}

		/// <summary>
		/// Выполняемая команда
		/// </summary>
		public TCommand Command { get; } = null;
	}
}
