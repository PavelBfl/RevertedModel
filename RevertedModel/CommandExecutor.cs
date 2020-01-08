using System;
using System.Collections.Generic;
using System.Text;

namespace RevertedModel
{
	/// <summary>
	/// Объект выполнения команд
	/// </summary>
	public abstract class CommandExecutor
	{
		/// <summary>
		/// Выполнить действия команды
		/// </summary>
		public abstract void Execute();
		/// <summary>
		/// Отменить действия команды
		/// </summary>
		public abstract void Undo();
	}
}
