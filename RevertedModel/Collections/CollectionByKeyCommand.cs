using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevertedModel.Collections
{
	/// <summary>
	/// Тип выполняемых команд над коллекцией с ключами
	/// </summary>
	public enum CollectionByKeyChanged
	{
		/// <summary>
		/// Удалить элемент
		/// </summary>
		Remove,
		/// <summary>
		/// Добавить элемент
		/// </summary>
		Insert,
		/// <summary>
		/// Обновить значение элемента
		/// </summary>
		Update,
	}
	/// <summary>
	/// Команда изменения коллекции с ключами
	/// </summary>
	/// <typeparam name="TKey">Тип ключа</typeparam>
	/// <typeparam name="TValue">Тип елемента коллекции</typeparam>
	public class CollectionByKeyCommand<TKey, TValue> : Command
	{
		public CollectionByKeyCommand(CollectionByKeyChanged action, TKey key, TValue newValue)
		{
			Action = action;
			Key = key;
			NewValue = newValue;
		}
		/// <summary>
		/// Действия выполняемые командой
		/// </summary>
		public CollectionByKeyChanged Action { get; } = (CollectionByKeyChanged)(-1);
		/// <summary>
		/// Ключ элемента в коллекции
		/// </summary>
		public TKey Key { get; } = default;
		/// <summary>
		/// Новое значение элемента
		/// </summary>
		public TValue NewValue { get; } = default;
	}
}
