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
	public class TrackObject
	{
		private const string COMMAND_DISPATCHER_NULL_MESSAGE = "Диспетчер команд не может принимать значение null";

		public TrackObject()
			: this(TrackDispatcher.Default)
		{

		}
		public TrackObject(TrackDispatcher trackDispatcher)
		{
			TrackDispatcher = trackDispatcher ?? throw new NullReferenceException(COMMAND_DISPATCHER_NULL_MESSAGE);
		}

		/// <summary>
		/// Диспетчер команд
		/// </summary>
		public TrackDispatcher TrackDispatcher { get; } = null;
		
		/// <summary>
		/// Выполнить команду
		/// </summary>
		/// <param name="commandExecutor">Объект выполнения команд</param>
		protected void Execute(CommandExecutor commandExecutor)
		{
			TrackDispatcher.AddAndExecute(commandExecutor);
		}
	}
}
