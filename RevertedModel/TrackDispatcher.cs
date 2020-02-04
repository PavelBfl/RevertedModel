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

		public TrackDispatcher(IOffsetTokenDispatcher offsetTokenDispatcher)
		{
			OffsetTokenDispatcher = offsetTokenDispatcher ?? throw new NullReferenceException(nameof(offsetTokenDispatcher));
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
		/// Диспетчер имеет текущий токен сдвига
		/// </summary>
		private bool CurrentTokenExists => commands.Any();
		/// <summary>
		/// Текущий токен сдвига
		/// </summary>
		private IComparable CurrentTokent
		{
			get
			{
				if (!CurrentTokenExists)
				{
					throw new TrackModelExeption(UNKNOWN_TOKEN);
				}
				return commands.Last().OffsetToken;
			}
		}

		/// <summary>
		/// Диспетчер токенов сдвига команд
		/// </summary>
		public IOffsetTokenDispatcher OffsetTokenDispatcher { get; } = default;
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
		/// Сдвинуть команды до указаного токена
		/// </summary>
		/// <param name="token">Токен до которого необходимо сдвинуть команды</param>
		public void Offset(IComparable token)
		{
			if (CurrentTokenExists)
			{
				var compareResult = token.CompareTo(CurrentTokent);
				if (compareResult > 0)
				{
					Offset(token, command => command.Execute(), undoCommands, commands);
				}
				else if (compareResult < 0)
				{
					Offset(token, command => command.Undo(), commands, undoCommands);
				}
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
			undoCommands.Clear();
			var token = OffsetTokenDispatcher.CreateToken();
			if (CurrentTokenExists && token.CompareTo(CurrentTokent) <= 0)
			{
				throw new OffsetTokenLessCurrentExeption();
			}
			commands.Push(new TokenExecutor(commandExecutor, token));
			commandExecutor.Execute();
		}

		/// <summary>
		/// Токен сохранения шага действия на линии сдвига
		/// </summary>
		private struct TokenExecutor
		{
			public TokenExecutor(CommandExecutor commandExecutor, IComparable offsetToken)
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
			public IComparable OffsetToken { get; }
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
