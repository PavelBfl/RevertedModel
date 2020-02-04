using System;
using System.Collections.Generic;
using System.Text;

namespace RevertedModel.Exceptions
{
	/// <summary>
	/// Базовое исключение для элементов командной модели
	/// </summary>
	public class TrackModelExeption : Exception
	{
		public TrackModelExeption()
		{
		}

		public TrackModelExeption(string message)
			: base(message)
		{
		}

		public TrackModelExeption(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
