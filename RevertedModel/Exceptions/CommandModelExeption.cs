using System;
using System.Collections.Generic;
using System.Text;

namespace RevertedModel.Exceptions
{
	/// <summary>
	/// Базовое исключение для элементов командной модели
	/// </summary>
	public class CommandModelExeption : Exception
	{
		public CommandModelExeption()
		{
		}

		public CommandModelExeption(string? message)
			: base(message)
		{
		}

		public CommandModelExeption(string? message, Exception? innerException)
			: base(message, innerException)
		{
		}
	}
}
