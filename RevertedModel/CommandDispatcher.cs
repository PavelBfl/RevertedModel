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
	public class CommandDispatcher
	{
		public CommandDispatcher(IOffsetTokenDispatcher offsetTokenDispatcher)
		{
			OffsetTokenDispatcher = offsetTokenDispatcher ?? throw new NullReferenceException(nameof(offsetTokenDispatcher));
		}

		/// <summary>
		/// Список выполненых команд
		/// </summary>
		private readonly Stack<TokenCommand> commands = new Stack<TokenCommand>();
		/// <summary>
		/// Список отменёных команд
		/// </summary>
		private readonly Stack<TokenCommand> undoCommands = new Stack<TokenCommand>();

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
					throw new Exception();
				}
				return commands.Last().OffsetToken;
			}
		}

		/// <summary>
		/// Диспетчер токенов сдвига команд
		/// </summary>
		public IOffsetTokenDispatcher OffsetTokenDispatcher { get; } = default!;

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
		private void Offset(object token, Action<CommandExecutor> commandExecutor, Stack<TokenCommand> giver, Stack<TokenCommand> taker)
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
			Add(commandExecutor);
			commandExecutor.Execute();
		}
		/// <summary>
		/// Добавить команду
		/// </summary>
		/// <param name="command">Команда</param>
		public void Add(CommandExecutor commandExecutor)
		{
			undoCommands.Clear();
			var token = OffsetTokenDispatcher.CreateToken();
			if (CurrentTokenExists && token.CompareTo(CurrentTokent) <= 0)
			{
				throw new OffsetTokenLessCurrentExeption();
			}
			commands.Push(new TokenCommand(commandExecutor, token));
		}

		private struct TokenCommand
		{
			public TokenCommand(CommandExecutor commandExecutor, IComparable offsetToken)
			{
				CommandExecutor = commandExecutor;
				OffsetToken = offsetToken;
			}

			public CommandExecutor CommandExecutor { get; set; }
			public IComparable OffsetToken { get; set; }
		}
	}
}
