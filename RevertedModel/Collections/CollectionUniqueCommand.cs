using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevertedModel.Collections
{
	/// <summary>
	/// Тип выполняемых команд над уникальной коллекцией
	/// </summary>
	public enum CollectionUniqueChanged
	{
		/// <summary>
		/// Добавить элемент
		/// </summary>
		Add,
		/// <summary>
		/// Удалить элемент
		/// </summary>
		Remove
	}
	/// <summary>
	/// Команда изменения колекции уникальных значений
	/// </summary>
	/// <typeparam name="T">Тип элемента коллекции</typeparam>
	public class CollectionUniqueCommand<T> : Command
	{
		public CollectionUniqueCommand(CollectionUniqueChanged action, T item)
		{
			Action = action;
			Item = item;
		}

		/// <summary>
		/// Тип выполняемой команды
		/// </summary>
		public CollectionUniqueChanged Action { get; } = (CollectionUniqueChanged)(-1);
		/// <summary>
		/// Обрабатываемый элемент коллекции
		/// </summary>
		public T Item { get; } = default;
	}
}
