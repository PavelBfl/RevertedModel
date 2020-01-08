using System;
using System.Collections.Generic;
using System.Text;

namespace RevertedModel
{
	/// <summary>
	/// Объекты выполнения команды намд объектом
	/// </summary>
	/// <typeparam name="TTarget">Тип целевого объекта</typeparam>
	/// <typeparam name="TCommand">Тип выполняемой команды</typeparam>
	public abstract class CommandExecutorTarger<TTarget, TCommand> : CommandExecutor<TCommand>
		where TTarget : CommandedObject
		where TCommand : Command
	{
		public CommandExecutorTarger(TTarget target, TCommand command)
			: base(command)
		{
			Target = target ?? throw new NullReferenceException(nameof(target));
		}

		/// <summary>
		/// Целевой объект
		/// </summary>
		public TTarget Target { get; } = default!;

		public sealed override void Execute()
		{
			using (Target.Disable())
			{
				ExecuteForce();
			}
		}
		protected abstract void ExecuteForce();
		public sealed override void Undo()
		{
			using (Target.Disable())
			{
				UndoForce();
			}
		}
		protected abstract void UndoForce();
	}
}
