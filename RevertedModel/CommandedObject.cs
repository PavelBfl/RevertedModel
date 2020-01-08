using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RevertedModel
{
	/// <summary>
	/// Объект выполнения команд
	/// </summary>
	public class CommandedObject
	{
		private const string COMMAND_DISPATCHER_NULL_MESSAGE = "Диспетчер команд не может принимать значение null";

		public CommandedObject(CommandDispatcher commandDispatcher)
		{
			CommandDispatcher = commandDispatcher ?? throw new NullReferenceException(COMMAND_DISPATCHER_NULL_MESSAGE);
		}

		/// <summary>
		/// Диспетчер команд
		/// </summary>
		public CommandDispatcher CommandDispatcher { get; } = default;
		/// <summary>
		/// Флаг записи действий объекта в общий стек комманд
		/// </summary>
		public bool CommandRecording { get; private set; } = true;
		/// <summary>
		/// Отключить запись действий в стек команд
		/// </summary>
		/// <returns>Дескриптор востановления записи действий</returns>
		internal IDisposable Disable()
		{
			return new DisableCommandRecordingToken(this);
		}
		/// <summary>
		/// Выполнить команду
		/// </summary>
		/// <param name="commandExecutor">Объект выполнения команд</param>
		protected void Execute(CommandExecutor commandExecutor)
		{
			CommandDispatcher.AddAndExecute(commandExecutor);
		}

		private class DisableCommandRecordingToken : IDisposable
		{
			public DisableCommandRecordingToken(CommandedObject commandedObject)
			{
				CommandedObject = commandedObject;
				if (CommandedObject.CommandRecording)
				{
					CommandedObject.CommandRecording = false;
					isCallback = true;
				}
			}

			public CommandedObject CommandedObject { get; } = default;

			private readonly bool isCallback = false;

			public void Dispose()
			{
				if (isCallback)
				{
					CommandedObject.CommandRecording = true;
				}
			}
		}
	}
}
