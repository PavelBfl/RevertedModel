using RevertedModel.Collections;
using System;
using System.Collections.Generic;
using System.Text;

namespace RevertedModel.Extensions
{
	/// <summary>
	/// Методы расширения для диспетчера отслеживания
	/// </summary>
	public static class TrackDispatcherExtension
	{
		/// <summary>
		/// Создать отслеживаемое значение на основе указаного диспетчера
		/// </summary>
		/// <typeparam name="T">Тип создаваемого значения</typeparam>
		/// <param name="trackDispatcher">Текущий диспетчер</param>
		/// <returns>Отслеживаемое значение</returns>
		public static ITrackValue<T> CreateValue<T>(this TrackDispatcher trackDispatcher)
		{
			return new TrackValue<T>(trackDispatcher);
		}
		/// <summary>
		/// Создать отслеживаемое значение на основе указаного диспетчера
		/// </summary>
		/// <typeparam name="T">Тип создаваемого значения</typeparam>
		/// <param name="trackDispatcher">Текущий диспетчер</param>
		/// <param name="initValue"></param>
		/// <returns>Отслеживаемое значение</returns>
		public static ITrackValue<T> CreateValue<T>(this TrackDispatcher trackDispatcher, T initValue)
		{
			return new TrackValue<T>(initValue, trackDispatcher);
		}

		/// <summary>
		/// Создать отслеживаемый список значений
		/// </summary>
		/// <typeparam name="T">Тип элемента списка</typeparam>
		/// <param name="trackDispatcher">Текущий диспетчер</param>
		/// <returns>Отслеживаемый список</returns>
		public static ITrackList<T> CreateList<T>(this TrackDispatcher trackDispatcher)
		{
			return new TrackList<T>(trackDispatcher);
		}
		/// <summary>
		/// Создать отслеживаемый список значений
		/// </summary>
		/// <typeparam name="T">Тип элемента списка</typeparam>
		/// <param name="trackDispatcher">Текущий диспетчер</param>
		/// <param name="items">Значения для инициализации</param>
		/// <returns>Отслеживаемый список</returns>
		public static ITrackList<T> CreateList<T>(this TrackDispatcher trackDispatcher, IEnumerable<T> items)
		{
			return new TrackList<T>(items, trackDispatcher);
		}

		/// <summary>
		/// Создать отслеживаемый словарь
		/// </summary>
		/// <typeparam name="TKey">Тип ключа</typeparam>
		/// <typeparam name="TValue">Тип значения</typeparam>
		/// <param name="trackDispatcher">Текущий диспетчер</param>
		/// <returns>Отслеживаемый словать</returns>
		public static ITrackDictionary<TKey, TValue> CreateDictionary<TKey, TValue>(this TrackDispatcher trackDispatcher)
		{
			return new TrackDictionary<TKey, TValue>(trackDispatcher);
		}
		/// <summary>
		/// Создать множество уникальных значений
		/// </summary>
		/// <typeparam name="T">Тип значения</typeparam>
		/// <param name="trackDispatcher">Текущий диспетчер</param>
		/// <returns>Множество уникальных значений</returns>
		public static ITrackSet<T> CreateSet<T>(this TrackDispatcher trackDispatcher)
		{
			return new TrackCollectionUnique<T>(trackDispatcher);
		}
	}
}
