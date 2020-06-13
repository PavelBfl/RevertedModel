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
		private readonly Stack<TokenExecutor> commands = new Stack<TokenExecutor>();
		/// <summary>
		/// Список отменёных команд
		/// </summary>
		private readonly Stack<TokenExecutor> undoCommands = new Stack<TokenExecutor>();
		/// <summary>
		/// Список используемых токенов
		/// </summary>
		private readonly HashSet<object> tokens = new HashSet<object>();

		/// <summary>
		/// Отчистить список отменёных команд
		/// </summary>
		private void ClearUndoCommands()
		{
			if (undoCommands.Any())
			{
				foreach (var command in undoCommands)
				{
					tokens.Remove(command.OffsetToken);
				}
				undoCommands.Clear();
			}
		}

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
			if (!tokens.Contains(token))
			{
				throw new TrackModelExeption();
			}
			if (commands.Select(item => item.OffsetToken).Contains(token))
			{
				Offset(token, command => command.Undo(), commands, undoCommands);
			}
			else if (undoCommands.Select(item => item.OffsetToken).Contains(token))
			{
				Offset(token, command => command.Execute(), undoCommands, commands);
			}
			else
			{
				throw new InvalidOperationException();
			}
		}
		private void Offset(object token, Action<CommandExecutor> commandExecutor, Stack<TokenExecutor> giver, Stack<TokenExecutor> taker)
		{
			while (giver.Any())
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
			ClearUndoCommands();
			var token = OffsetTokenDispatcher.CreateToken();
			if (tokens.Contains(token))
			{
				throw new OffsetTokenLessCurrentExeption();
			}
			tokens.Add(token);
			commands.Push(new TokenExecutor(commandExecutor, token));
			commandExecutor.Execute();
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
