using System;
using System.Collections.Generic;
using System.Text;

namespace RevertedModel
{
	/// <summary>
	/// Объект скалярного значения
	/// </summary>
	/// <typeparam name="T">Тип значения</typeparam>
	public class CommandedValue<T> : CommandedObject
	{
		public CommandedValue(CommandDispatcher commandDispatcher)
			: base(commandDispatcher)
		{
		}
		public CommandedValue(T initValue, CommandDispatcher commandDispatcher)
			: base(commandDispatcher)
		{
			Value = initValue;
		}

		/// <summary>
		/// Текущее значение
		/// </summary>
		public T Value
		{
			get => value;
			set
			{
				if (CommandRecording)
				{
					Execute(new ValueExecutor<T>(this, new ValueCommand<T>(value)));
				}
				else
				{
					this.value = value;
				}
			}
		}
		private T value = default!;
	}
}
