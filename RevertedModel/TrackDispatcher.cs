using RevertedModel.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevertedModel
{
	/// <summary>
	/// Диспетчер команд
	/// </summary>
	public class TrackDispatcher
	{
		private const string UNKNOWN_TOKEN = "Не удалось получить значение текущего токена сдвига т.к. отсутствуют записаные действия";

		/// <summary>
		/// Диспетчер отслеживания по умолчанию
		/// </summary>
		public static TrackDispatcher Default => @default ?? (@default = new TrackDispatcher(new DefaultOffsetTokenDispatcher()));
		private static TrackDispatcher @default = null;

		public TrackDispatcher(ITrackTokenProvider offsetTokenDispatcher)
		{
			OffsetTokenDispatcher = offsetTokenDispatcher ?? throw new ArgumentNullException(nameof(offsetTokenDispatcher));
		}

		/// <summary>
		/// Список выполненых команд
		/// </summary>
		private readonly TrackCommands commands = new TrackCommands();
		/// <summary>
		/// Список отменёных команд
		/// </summary>
		private readonly TrackCommands undoCommands = new TrackCommands();

		/// <summary>
		/// Список используемых токенов
		/// </summary>
		public IEnumerable<object> Tokens => commands.Tokens.Concat(undoCommands.Tokens);
		/// <summary>
		/// Текущий ключевой токен
		/// </summary>
		public object CurrentToken => commands.Peek().OffsetToken;
		/// <summary>
		/// Диспетчер токенов сдвига команд
		/// </summary>
		public ITrackTokenProvider OffsetTokenDispatcher { get; } = default;
		/// <summary>
		/// Получить значение активности диспетчера, определяет производится ли запись изменений
		/// </summary>
		public bool IsEnable { get; private set; } = true;
		/// <summary>
		/// Отключить запись действий
		/// </summary>
		/// <returns>Токен востановления активностии диспетчера</returns>
		public IDisposable Disable()
		{
			return new DisableToken(this);
		}
		/// <summary>
		/// Очистить все шаги отслеживания
		/// </summary>
		public void Clear()
		{
			commands.Clear();
			undoCommands.Clear();
		}


		/// <summary>
		/// Сдвинуть команды до указаного токена
		/// </summary>
		/// <param name="token">Токен до которого необходимо сдвинуть команды</param>
		public void Offset(object token)
		{
			if (commands.Contains(token))
			{
				Offset(token, command => command.Undo(), commands, undoCommands);
			}
			else if (undoCommands.Contains(token))
			{
				Offset(token, command => command.Execute(), undoCommands, commands);
			}
			else
			{
				throw new InvalidOperationException();
			}
		}
		private void Offset(object token, Action<CommandExecutor> commandExecutor, TrackCommands giver, TrackCommands taker)
		{
			while (giver.Count > 0)
			{
				var tokenCommand = giver.Pop();

				commandExecutor(tokenCommand.CommandExecutor);

				taker.Push(tokenCommand);
				if (tokenCommand.OffsetToken.Equals(token))
				{
					return;
				}
			}
		}

		/// <summary>
		/// Добавить и выполнить команду
		/// </summary>
		/// <param name="commandExecutor">Команда</param>
		public void AddAndExecute(CommandExecutor commandExecutor)
		{
			var token = OffsetTokenDispatcher.CreateToken();
			if (commands.Contains(token) || undoCommands.Contains(token))
			{
				throw new OffsetTokenLessCurrentExeption();
			}
			commands.Push(new TokenExecutor(commandExecutor, token));
			commandExecutor.Execute();
		}

		/// <summary>
		/// Очередь сохранённых комманд
		/// </summary>
		private class TrackCommands
		{
			/// <summary>
			/// Список комманд
			/// </summary>
			private readonly Stack<TokenExecutor> commands = new Stack<TokenExecutor>();

			/// <summary>
			/// Список используемых токенов
			/// </summary>
			public IEnumerable<object> Tokens => tokens;
			private readonly HashSet<object> tokens = new HashSet<object>();
			/// <summary>
			/// Количество элементов коллекции
			/// </summary>
			public int Count => commands.Count;
			/// <summary>
			/// Проверка содержания ключевого токена в очереди
			/// </summary>
			/// <param name="token">Искомый токен</param>
			/// <returns>true если токен содержится в очереди, иначе false</returns>
			public bool Contains(object token)
			{
				return tokens.Contains(token);
			}
			/// <summary>
			/// Добавить элемент вызова в стек
			/// </summary>
			/// <param name="executor">Токен действия</param>
			public void Push(TokenExecutor executor)
			{
				if (Contains(executor.OffsetToken))
				{
					throw new TrackModelExeption();
				}
				tokens.Add(executor.OffsetToken);
				commands.Push(executor);
			}
			/// <summary>
			/// Получить первый элемент в стеке, и удалить его из стека
			/// </summary>
			/// <returns>Элемент выполнения команды</returns>
			public TokenExecutor Pop()
			{
				var result = commands.Pop();
				if (!tokens.Remove(result.OffsetToken))
				{
					throw new InvalidOperationException();
				}
				return result;
			}
			/// <summary>
			/// Получить первый элемент в стеке
			/// </summary>
			/// <returns>Элемент выполнения команды</returns>
			public TokenExecutor Peek()
			{
				return commands.Peek();
			}
			/// <summary>
			/// Отчистить стек
			/// </summary>
			public void Clear()
			{
				tokens.Clear();
				commands.Clear();
			}
		}
		/// <summary>
		/// Токен сохранения шага действия на линии сдвига
		/// </summary>
		private struct TokenExecutor
		{
			public TokenExecutor(CommandExecutor commandExecutor, object offsetToken)
			{
				CommandExecutor = commandExecutor;
				OffsetToken = offsetToken;
			}
			/// <summary>
			/// Объект выполнения команды
			/// </summary>
			public CommandExecutor CommandExecutor { get; }
			/// <summary>
			/// Токен позиции действия на линии сдвига
			/// </summary>
			public object OffsetToken { get; }
		}
		/// <summary>
		/// Токен отключения диспетчера
		/// </summary>
		private class DisableToken : IDisposable
		{
			public DisableToken(TrackDispatcher trackDispatcher)
			{
				TrackDispatcher = trackDispatcher ?? throw new NullReferenceException();

				if (TrackDispatcher.IsEnable)
				{
					TrackDispatcher.IsEnable = false;
					IsCallback = true;
				}
			}
			/// <summary>
			/// Диспетчер трекинга команд
			/// </summary>
			public TrackDispatcher TrackDispatcher { get; } = null;
			/// <summary>
			/// Флаг необходимости включения дисптчера по завершении работы токена
			/// </summary>
			public bool IsCallback { get; } = false;
			/// <summary>
			/// завершение работы токена
			/// </summary>
			public void Dispose()
			{
				if (IsCallback)
				{
					TrackDispatcher.IsEnable = true;
				}
			}
		}
	}
}
