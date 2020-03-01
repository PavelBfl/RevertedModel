using System;
using System.Collections.Generic;
using System.Text;

namespace RevertedModel
{
	/// <summary>
	/// Объект скалярного значения
	/// </summary>
	/// <typeparam name="T">Тип значения</typeparam>
	public class TrackValue<T> : TrackObject, ITrackValue<T>
	{
		public TrackValue()
			: base()
		{

		}
		public TrackValue(TrackDispatcher commandDispatcher)
			: base(commandDispatcher)
		{
		}
		public TrackValue(T initValue)
			: base()
		{
			Value = initValue;
		}
		public TrackValue(T initValue, TrackDispatcher commandDispatcher)
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
				if (TrackDispatcher.IsEnable)
				{
					Execute(new ValueExecutor<T>(this, new ValueCommand<T>(value)));
				}
				else
				{
					this.value = value;
				}
			}
		}
		private T value = default;
	}
}
