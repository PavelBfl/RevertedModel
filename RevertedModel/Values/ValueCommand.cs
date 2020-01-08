using System;
using System.Collections.Generic;
using System.Text;

namespace RevertedModel
{
	/// <summary>
	/// Команда изменения скалярного значения
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ValueCommand<T> : Command
	{
		public ValueCommand(T newValue)
		{
			NewValue = newValue;
		}
		/// <summary>
		/// Новое значение
		/// </summary>
		public T NewValue { get; } = default!;
	}
}
